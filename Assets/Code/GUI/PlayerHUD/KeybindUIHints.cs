using System;
using Code.GUI.PlayerHUD;
using Code.Player.Interaction;
using UnityEngine;

namespace Code.GUI
{
    public class KeybindUIHints : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _dropItemHint;
        [SerializeField] private CanvasGroup _useItemHint;

        private HotbarInteraction _playerHotbar;
        
        private void Awake()
        {
            var holder = GetComponentInParent<PlayerHUDVariableHolder>();
            _playerHotbar = holder.PlayerHotbar;
        }

        private void Update()
        {
            DisplayDropItemHint();
            DisplayUseItemHint();
        }

        void DisplayDropItemHint()
        {
            _dropItemHint.alpha = _playerHotbar.IsPlayerHoldingItem ? 1 : 0;
        }

        void DisplayUseItemHint()
        {
            _useItemHint.alpha = _playerHotbar.IsPlayerHoldingItem ? 1 : 0;
        }
    }
}