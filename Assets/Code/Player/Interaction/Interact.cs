using System;
using Code.Debug;
using Code.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Code.Player.Interaction
{
    public class Interact : MonoBehaviour
    {
        [Header("Interact")]
        [SerializeField] float _interactDistance;
        [SerializeField] LayerMask _excludeLayers;
        
        [Header("Input")] 
        [SerializeField] InputActionAsset _inputActionAsset;
        
        [SerializeField] Camera _playerCamera;
        
        private Inventory.Inventory _playerInventory;
        private InputAction _interactAction;

        private Item _currentlySelectedItem;
        
        private Player _player;

        private void Awake()
        {
            _interactAction = _inputActionAsset.FindAction("Interact");

            _interactAction.started += OnInteractionStarted;
            _interactAction.performed += OnInteractionPerformed;
            _interactAction.canceled += OnInteractionCancelled;
        }

        private void Start()
        {
            _player = GetComponent<Player>();
            _playerInventory = _player.Inventory;
        }

        private void OnEnable()
        {
            _interactAction.Enable();
        }

        private void OnDisable()
        {
            _interactAction.Disable();
        }

        private void Update()
        {
            CheckForInteractables();

        }

        private void FixedUpdate()
        {
            //CheckForInteractables();
        }

        void OnInteractionStarted(InputAction.CallbackContext context) { }
        void OnInteractionCancelled(InputAction.CallbackContext context) { }
        
        void OnInteractionPerformed(InputAction.CallbackContext context)
        {
            if (context.interaction is TapInteraction)
            {
                // Have to check if at the end of the action we're still hovering what we want to use
                if (_currentlySelectedItem != null)
                {
                    _playerInventory.AddInventoryItem(_currentlySelectedItem.GetItemData(), 1);
                    _currentlySelectedItem.OnPickUp();
                }
            }
        }

        void CheckForInteractables()
        {
            var startPos = _playerCamera.transform.position;
            var direction = _playerCamera.transform.forward;
            
            if(DebugDrawMenu.DrawInteractions)
                UnityEngine.Debug.DrawLine(startPos, direction * _interactDistance, Color.green);
            
            var rayHit = Physics.Raycast(startPos, direction, out var hit, _interactDistance, ~_excludeLayers);

            if (rayHit == false)
            {
                DeselectItem();
                return;
            }
            
            UnityEngine.Debug.Log(hit.collider?.name);
            
            if (hit.collider.gameObject.TryGetComponent<Item>(out var usableItem) == false)
            {
                DeselectItem();
                return;
            }
            
            if(DebugDrawMenu.DrawInteractions)
                UnityEngine.Debug.DrawLine(hit.collider.transform.position, hit.collider.transform.position + new Vector3(0, 10, 0), Color.green);
            
            // We're looking so we call the event
            if(usableItem.IsWorldSelect == false)
                usableItem.OnWorldSelect(_player);
            
            _currentlySelectedItem = usableItem;
        }

        void DeselectItem()
        {
            if (_currentlySelectedItem != null)
            {
                _currentlySelectedItem.OnWorldDeSelect();
                _currentlySelectedItem = null;
            }
        }
    }
}