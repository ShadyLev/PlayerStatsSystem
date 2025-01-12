using System;
using System.Collections.Generic;
using Code.Items;
using ScriptableObjects;

namespace Code.Player.Inventory
{
    public class Inventory
    {
        private const int HOTBAR_SIZE = 2;

        // Slot - Item
        public Action<int, ItemSO> OnHotbarItemAdded;
        public Action<int, ItemSO> OnHotbarItemRemoved;
        
        // Item - New amount
        public Action<ItemSO, int> InventoryItemAmountChanged;
        
        /// <summary>
        /// I'm not convinced I need a full inventory stash, keeping this for now.
        /// Might remove and keep hotbar as the only inventory type.
        /// </summary>
        // Item to amount
        private Dictionary<ItemSO, int> _inventory = new();
        
        // Slot ID to Item
        private ItemSO[] _hotbar = new ItemSO[HOTBAR_SIZE];
        
        public Dictionary<ItemSO, int> GetInventory()
        {
            return _inventory;
        }

        public bool GetHotbarItem(int index, out ItemSO item)
        {
            item = default;
            if (_hotbar.Length <= index)
            {
                UnityEngine.Debug.LogError($"Hotbar index {index} out of range of hotbar array length {_hotbar.Length}");
                return false;
            }

            if (_hotbar[index] == null)
                return false;
            
            item = _hotbar[index];
            return true;
        }

        public void Init()
        {
            
        }
        
        public void AddInventoryItem(ItemSO item, int amount)
        {
            // Add item to inventory
            if (_inventory.TryAdd(item, amount) == false)
            {
                _inventory[item] += amount;
                InventoryItemAmountChanged.Invoke(item, _inventory[item]);
            }
            
            // Assign Item to Hotbar
            for (int i = 0; i < _hotbar.Length; i++)
            {
                if (_hotbar[i] == null || _hotbar[i].Id == item.Id)
                {
                    _hotbar[i] = item;
                    OnHotbarItemAdded.Invoke(i, item);
                    break;
                }
            }
        }

        public void RemoveInventoryItem(ItemSO item, int amount)
        {
            // Remove Item from Inventory
            if (_inventory.ContainsKey(item) == false)
            {
                UnityEngine.Debug.LogError($"Trying to remove item {item} that is not in the inventory");
                return;
            }
            
            _inventory[item] -= amount;
            InventoryItemAmountChanged.Invoke(item, _inventory[item]);

            if (_inventory[item] > 0)
                return;
            
            // If we fully removed the item from the inventory, also remove from hotbar
            _inventory.Remove(item);
            for (int i = 0; i < _hotbar.Length; i++)
            {
                if (_hotbar[i] == item)
                {
                    OnHotbarItemRemoved.Invoke(i, item);
                    _hotbar[i] = null;
                    break;
                }
            }
        }

        public int GetItemAmount(ItemSO item)
        {
            return _inventory[item];
        }

        public bool HasItemInInventory(ItemSO item)
        {
            return _inventory.ContainsKey(item);
        }
    }
}