using UnityEngine;
using UnityEngine.Serialization;

namespace Shady.Game.Camera
{
    public class PlayerHeadBob : MonoBehaviour
    {
        [SerializeField] private bool _enableHeadBob;
        [SerializeField] private bool _enableStabilization;
        
        [Header("Head Bob Settings")]
        [SerializeField, Range(0, 0.1f)] private float _walkAmplitude = 0.0063f;
        [SerializeField, Range(0, 0.1f)] private float _sprintAmplitude = 0.008f;
        [SerializeField, Range(0, 0.1f)] private float _jumpAmplitude = 0.01f;
        
        [SerializeField, Range(0, 30)] private float _walkFrequency = 8f;
        [SerializeField, Range(0, 30)] private float _sprintFrequency = 15f;
        [SerializeField, Range(0, 30)] private float _jumpFrequency = 50f;
        
        [SerializeField] private float _resetSpeed = 1f;
        [SerializeField] private float _stablizationTarget = 15f;
        [SerializeField] private float _speedThreshold = 3.0f;
        
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _cameraHolder;

        private float _amplitude = 0.0063f;
        private float _frequency = 8f;
        
        private Vector3 _startPosition;
        private CharacterController _characterController;
        private PlayerMovement.PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement.PlayerMovement>();
            _characterController = GetComponent<CharacterController>();
            _startPosition = _camera.localPosition;
        }

        private void Update()
        {
            if(_enableHeadBob == false)
                return;
            
            AdjustHeadBob();            
            CheckMotion();
            ResetPosition();
            
            Debug.Log("TEst");
            if(_enableStabilization)
                _camera.LookAt(FocusTarget());
        }

        private void AdjustHeadBob()
        {
            // TODO: Could be a enum set by PlayerMovement instead, then this will be a switch - cleaner and only one place determines it
            if (_playerMovement.IsMoving)
            {
                _amplitude = _walkAmplitude;
                _frequency = _walkFrequency;
            }
            else if (_playerMovement.IsSprinting)
            {
                _amplitude = _sprintAmplitude;
                _frequency = _sprintFrequency;
            }
            else if(_playerMovement.IsJumping)
            {
                _amplitude = _jumpAmplitude;
                _frequency = _jumpFrequency;
            }
        }
        
        private Vector3 FocusTarget()
        {
            var pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y,
                transform.position.z);
            pos += _cameraHolder.forward * _stablizationTarget;
            return pos;
        }

        private void PlayMotion(Vector3 motion)
        {
            _camera.localPosition += motion;
        }
        
        private void CheckMotion()
        {
            var speed = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).magnitude;
            
            if(speed < _speedThreshold)
                return;
            
            if(_characterController.isGrounded == false)
                return;
            
            PlayMotion(FootstepMotion());
        }
        
        private Vector3 FootstepMotion()
        {
            var pos = Vector3.zero;
            pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
            pos.x += Mathf.Cos(Time.time * _frequency / 2) * _amplitude / 2;
            return pos;
        }

        private void ResetPosition()
        {
            if(_camera.localPosition == _startPosition) 
                return;
            
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPosition, _resetSpeed * Time.deltaTime);
        }
    }
}