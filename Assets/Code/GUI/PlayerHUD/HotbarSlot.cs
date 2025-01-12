using System;
using Code.Player.Interaction;
using Code.Player.Inventory;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.PlayerHUD
{
    public class HotbarSlot : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _itemAmountLabel;
        [SerializeField] private TextMeshProUGUI _slotIndexLabel;
        [SerializeField] private int _hotBarSlotIndex;
        [SerializeField] private Image _slotOutline;
        
        private Player.Player _player;
        private Inventory _playerInventory;
        private ItemSO _slotItem;
        
        private void Start()
        {
            var holder = GetComponentInParent<PlayerHUDVariableHolder>();
            _player = holder.Player;
            
            _playerInventory = _player.Inventory;
            
            // Hotbar array starts from 0, so we display +1. Not super clean but this is purely cosmetic.
            var labelSlotIndex = _hotBarSlotIndex + 1;
            _slotIndexLabel.SetText(labelSlotIndex.ToString());

            _player.Inventory.OnHotbarItemAdded += AssignHotbarItem;
            _player.Inventory.OnHotbarItemRemoved += RemoveHotbarItem;
            _player.Inventory.InventoryItemAmountChanged += UpdateItemCount;
            
            var hotbarInteraction = _player.gameObject.GetComponent<HotbarInteraction>();
            hotbarInteraction.OnSlotSelectionChanged += SelectSlot;
        }

        private void SelectSlot(int slotIndex)
        {
            if (slotIndex != _hotBarSlotIndex)
            {
                _slotOutline.enabled = false;
                return;
            }

            _slotOutline.enabled = true;
        }

        void AssignHotbarItem(int slot, ItemSO item)
        {
            if(slot != _hotBarSlotIndex)
                return;
            
            if (item == null)
            {
                ClearHotbarSlot();
            }

            var itemAmount = _playerInventory.GetItemAmount(item);

            _slotItem = item;
            _itemImage.sprite = item.Icon;
            _itemAmountLabel.SetText(itemAmount.ToString());
        }

        void RemoveHotbarItem(int slot, ItemSO item)
        {
            if(slot != _hotBarSlotIndex)
                return;
            
            ClearHotbarSlot();
        }

        void UpdateItemCount(ItemSO item, int newAmont)
        {
            if(_slotItem != item)
                return;
            
            _itemAmountLabel.SetText(newAmont.ToString());
        }

        void ClearHotbarSlot()
        {
            _slotItem = null;
            _itemImage.sprite = null;
            _itemAmountLabel.SetText("");
        }
    }
}