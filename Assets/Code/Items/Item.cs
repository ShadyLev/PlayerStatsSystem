using System;
using System.Collections.Generic;
using Code.Player;
using Code.PlayerStats;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Items
{
    /// <summary>
    /// Items can be used on the spot (Hold E) or put in small inventory (1,2,3,4,5)
    /// </summary>
    public abstract class Item : MonoBehaviour
    {
        public bool IsHeld
        {
            get => IsHeld;
            set
            {
                OnItemHeld(value);
            }
        }

        public bool IsWorldSelect { get; set; }
        
        [SerializeField] private ItemSO ItemData;
        [SerializeField] private Canvas PickupPrompCanvas;
        [SerializeField] private TextMeshProUGUI ItemNameLabel;
        
        // [Header("Effect")]
        // // This can easly be a struct, make it a list so we can apply multiple effects
        // [SerializeReference] protected List<StatEffect> Effects = new();
        
        private Rigidbody _rb;
        private Collider _collider;
        private Transform _selectedEntityTransform;
        
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            ItemNameLabel.SetText(ItemData.Name);
        }

        private void Update()
        {
            if (IsWorldSelect)
            {
                PickupPrompCanvas.transform.LookAt(_selectedEntityTransform);
            }
        }

        public ItemSO GetItemData()
        {
            return ItemData;
        }
        public void OnPickUp()
        {
            Destroy(gameObject);
        }
        public abstract void OnUse(PlayerEntity entity);

        public void OnWorldSelect(PlayerEntity entity)
        {
            IsWorldSelect = true;
            PickupPrompCanvas.enabled = true;
            _selectedEntityTransform = entity.transform;
        }

        public void OnWorldDeSelect()
        {
            IsWorldSelect = false;
            PickupPrompCanvas.enabled = false;
        }

        private void OnItemHeld(bool isHeld)
        {
            _rb.isKinematic = isHeld;
            _collider.enabled = isHeld == false;
        }
    }
}