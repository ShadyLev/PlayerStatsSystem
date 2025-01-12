using System;
using Code.Items;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code.Player.Interaction
{
    public class HotbarInteraction : MonoBehaviour
    {
        [Header("Input")] 
        [SerializeField] InputActionAsset _inputActionAsset;

        [Header("Item Hold Transform")]
        [SerializeField] Transform holdPosition;
        
        [Header("Drop Item Force")]
        [SerializeField] float dropItemForce = 15f;

        public Action<int> OnSlotSelectionChanged;

        public bool IsPlayerHoldingItem => _selectedItem != null;

        // Player
        private Player _player;
        private Inventory.Inventory _playerInventory;
        private CharacterController _playerCC;
        
        // Input actions
        private InputAction _slot1Action;
        private InputAction _slot2Action;
        private InputAction _slot3Action;
        private InputAction _toggleSlotAction;
        private InputAction _dropItemAction;
        private InputAction _useHotbarItemAction;

        // Currently selected hotbar slot Index, if -1 no slot is selected
        private int _currentlySelectedHotbarSlot = -1;
        
        // Reference to the currently selected Item
        private Item _selectedItem;
        
        private void Awake()
        {
            _slot1Action = _inputActionAsset.FindAction("HotbarSlot1");
            _slot2Action = _inputActionAsset.FindAction("HotbarSlot2");
            _slot3Action = _inputActionAsset.FindAction("HotbarSlot3");
            _toggleSlotAction = _inputActionAsset.FindAction("ToggleHotbarSlot");
            _dropItemAction = _inputActionAsset.FindAction("DropItem");
            _useHotbarItemAction = _inputActionAsset.FindAction("LeftClick");

            _slot1Action.performed += OnSlot1Selected;
            _slot2Action.performed += OnSlot2Selected;
            _slot3Action.performed += OnSlot3Selected;
            _dropItemAction.performed += DropItem;
            _useHotbarItemAction.performed += UseHotbarItem;
        }

        private void Start()
        {
            // Like fr this should be maybe injected?
            _player = GetComponent<Player>();
            _playerInventory = _player.Inventory;
            _playerCC = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _slot1Action.Enable();
        }

        private void OnDisable()
        {
            _slot1Action.Disable();
        }

        void DropItem(InputAction.CallbackContext context)
        {
            if(_currentlySelectedHotbarSlot == -1 || _selectedItem == null)
                return;
            
            _playerInventory.RemoveInventoryItem(_selectedItem.GetItemData(), 1);

            if (_playerInventory.GetHotbarItem(_currentlySelectedHotbarSlot, out var itemData))
            {
                var droppedGo = GameObject.Instantiate(itemData.Prefab, holdPosition.position, Quaternion.identity);
                var goRb = droppedGo.GetComponent<Rigidbody>();
                goRb.linearVelocity = _playerCC.velocity;
                goRb.AddForce(holdPosition.forward * dropItemForce, ForceMode.Impulse);
            }
            else
            {
                var goRb = _selectedItem.GetComponent<Rigidbody>();
                goRb.isKinematic = false; // We're dropping the currently holding item which is kinematic
                goRb.linearVelocity = _playerCC.velocity;
                goRb.AddForce(holdPosition.forward * dropItemForce, ForceMode.Impulse);
                DeselectCurrentSlot(); 
            }
        }
        
        void OnSlot1Selected(InputAction.CallbackContext context)
        {
            OnSlotSelected(context, 0);
        }
        
        void OnSlot2Selected(InputAction.CallbackContext context)
        {
            OnSlotSelected(context, 1);
        }
        
        void OnSlot3Selected(InputAction.CallbackContext context)
        {
            OnSlotSelected(context, 2);
        }
        
        void OnSlotSelected(InputAction.CallbackContext context, int index)
        {
            // Trying to swap to the same hotbar slot
            if(_currentlySelectedHotbarSlot == index)
                return;
            
            // Nothing in target hotbar slot
            if(_playerInventory.GetHotbarItem(index, out _) == false)
                return;
            
            _currentlySelectedHotbarSlot = index;
            OnSlotSelectionChanged.Invoke(_currentlySelectedHotbarSlot);
            
            // TODO: Add GO pooling
            // If we already hold an item from a hotbar, destroy it
            if (_selectedItem != null)
            {
                Destroy(_selectedItem);
            }
            
            if (_playerInventory.GetHotbarItem(index, out var itemData) == false)
            {
                return;
            }
            var prefabInstance = GameObject.Instantiate(itemData.Prefab, holdPosition);
            _selectedItem = prefabInstance.GetComponent<Item>();
            _selectedItem.IsHeld = true;
        }
        
        private void UseHotbarItem(InputAction.CallbackContext context)
        {
            if(_selectedItem == null)
                return;
            
            // Get animation information
            
            // Play animation
            
            _selectedItem.OnUse(_player);
            _playerInventory.RemoveInventoryItem(_selectedItem.GetItemData(), 1);

            if (HasItemInSlot(_currentlySelectedHotbarSlot) == false)
            {
                Destroy(_selectedItem.gameObject);
                DeselectCurrentSlot();
            }
        }

        private bool HasItemInSlot(int slotIndex)
        {
            return _playerInventory.GetHotbarItem(slotIndex, out _);
        }

        void DeselectCurrentSlot()
        {
            // We dropped everything from the inventory 
            _currentlySelectedHotbarSlot = -1;
            OnSlotSelectionChanged.Invoke(_currentlySelectedHotbarSlot);
                
            _selectedItem.IsHeld = false;
            _selectedItem.transform.parent = null;
            _selectedItem = null;
        }
    }
}