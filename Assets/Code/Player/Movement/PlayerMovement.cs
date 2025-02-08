using System;
using Code.Player;
using Code.PlayerStats;
using Shady.Game.PlayerSettings;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shady.Game.PlayerMovement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] 
        [SerializeField] private float _gravity = 12f;
        [SerializeField] private float _walkSpeed = 5f;
        [SerializeField] private float _sprintMultiplier = 2f;
        [SerializeField] private float _jumpForce = 5f;
        
        [Header("Stamina")]
        [SerializeField] private float _sprintModifierValue = -15f;
        [SerializeField] private float _jumpModifierValue = -30f;

        [Header("Looking")] 
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private float _verticalLookRange = 80f;

        [Header("Interactions")]
        [SerializeField] private float _itemPushPower = 5f;
        
        [Header("Audio")] [SerializeField] private float _walkStepInterval = 0.5f;
        [SerializeField] private float _sprintStepInterval = 0.3f;
        [SerializeField] private float _velocityThreshold = 2f;

        [Header("Input")] [SerializeField] private InputActionAsset _inputActionAsset;

        // Public properties
        public bool IsMoving => _isMoving;
        public bool IsJumping => _isJumping;
        public bool IsSprinting => _isSprinting;

        // Input privates
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private Vector2 _moveInput;
        private Vector2 _lookInput;

        // Movement privates
        private CharacterController _characterController;
        private Vector3 _currentMovement;
        private bool _isMoving;
        private bool _isSprinting;
        private bool _isJumping;
        private float _verticalRotation;

        // Audio privates
        private float _nextStepTime;
        
        // Player
        private Stats _playerStats;
        
        // Stamina
        private bool _sprintKeyPressed;
        private bool _appliedStamina;
        private BasicStatModifier _sprintModifier;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _moveAction = _inputActionAsset.FindAction("Move");
            _lookAction = _inputActionAsset.FindAction("Look");
            _jumpAction = _inputActionAsset.FindAction("Jump");
            _sprintAction = _inputActionAsset.FindAction("Sprint");

            _moveAction.performed += context => _moveInput = context.ReadValue<Vector2>();
            _moveAction.canceled += _ => _moveInput = Vector2.zero;

            _lookAction.performed += context => _lookInput = context.ReadValue<Vector2>();
            _lookAction.canceled += _ => _lookInput = Vector2.zero;
            
            _sprintAction.performed += _ => _sprintKeyPressed = true;
            _sprintAction.canceled += _ => _sprintKeyPressed = false;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Init stamina modifiers
            _sprintModifier = new BasicStatModifier(StatType.Stamina, ModifierType.CoTModifier, 0, (v) => v + _sprintModifierValue);
        }

        private void Start()
        {
            _playerStats = GetComponent<Player>().Stats;
        }

        private void OnEnable()
        {
            _moveAction.Enable();
            _lookAction.Enable();
            _jumpAction.Enable();
            _sprintAction.Enable();
        }

        private void OnDisable()
        {
            _moveAction.Disable();
            _lookAction.Disable();
            _jumpAction.Disable();
            _sprintAction.Disable();
        }

        void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleFootsteps();
        }

        void HandleMovement()
        {
            _isSprinting = _sprintAction.ReadValue<float>() > 0;

            if (_playerStats.GetStatValueFromType(StatType.Stamina) <= 0)
                _isSprinting = false;
            
            HandleSprintStamina();
            
            var speedMultiplier = _isSprinting ? _sprintMultiplier : 1f;
            var verticalSpeed = _moveInput.y * _walkSpeed * speedMultiplier;
            var horizontalSpeed = _moveInput.x * _walkSpeed * speedMultiplier;

            var movementVector = new Vector3(horizontalSpeed, 0, verticalSpeed);
            movementVector = transform.rotation * movementVector;

            HandleGravityAndJumping();

            _currentMovement.x = movementVector.x;
            _currentMovement.z = movementVector.z;

            _characterController.Move(_currentMovement * Time.deltaTime);
            _isMoving = _moveInput.y != 0 || _moveInput.x != 0;
        }

        private void HandleGravityAndJumping()
        {
            var isGrounded = _characterController.isGrounded;
            var hasStaminaToJump = _playerStats.GetStatValueFromType(StatType.Stamina) >= math.abs(_jumpModifierValue);
            if (isGrounded)
            {
                _currentMovement.y = -0.5f;

                if (_jumpAction.triggered && hasStaminaToJump)
                {
                    _isJumping = true;
                    _currentMovement.y = _jumpForce;
                    HandleJumpStamina();
                }
            }
            else
            {
                _isJumping = false;
                _currentMovement.y -= _gravity * Time.deltaTime;
            }
        }

        void HandleRotation()
        {
            var mouseXRotation = _lookInput.x * PlayerControlsSettings.Instance.MouseSensitivity;
            transform.Rotate(0, mouseXRotation, 0);

            _verticalRotation -= _lookInput.y * PlayerControlsSettings.Instance.MouseSensitivity;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalLookRange, _verticalLookRange);

            _cameraHolder.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        }

        void HandleFootsteps()
        {
            var currentStepInterval = _sprintAction.ReadValue<float>() > 0 ? _sprintStepInterval : _walkStepInterval;

            if (_characterController.isGrounded && _isMoving && Time.time > _nextStepTime &&
                _characterController.velocity.magnitude > _velocityThreshold)
            {
                // Play sounds
                _nextStepTime = Time.time + currentStepInterval;
            }
        }

        void HandleSprintStamina()
        {
            if(_characterController.velocity.magnitude <= 0)
                return;
            
            if (_sprintKeyPressed && _appliedStamina == false && _isSprinting)
            {
                _appliedStamina = true;
                _sprintModifier.MarkedForRemoval = false;
                _playerStats.Mediator.AddModifier(_sprintModifier);
            }

            if (_sprintKeyPressed == false && _appliedStamina)
            {
                _appliedStamina = false;
                _sprintModifier.MarkedForRemoval = true;
            }
        }

        void HandleJumpStamina()
        {
            _playerStats.ModifyStat(StatType.Stamina, _jumpModifierValue);
        }
        
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var body = hit.collider.attachedRigidbody;

            // no rigidbody
            if (body == null || body.isKinematic)
            {
                return;
            }

            if (hit.moveDirection.y < -0.3)
            {
                return;
            }
            
            var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.linearVelocity = pushDir * _itemPushPower;
        }
    }
}
