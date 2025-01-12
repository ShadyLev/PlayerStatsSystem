using System;
using Code.PlayerStats;
using TMPro;
using UnityEngine;

namespace Code.GUI.PlayerHUD
{
    public class PlayerStatusDebugDisplay : MonoBehaviour
    {
        private const string StringFormatType = "F0";
        
        [Header("Status Labels")]
        [SerializeField] private TextMeshProUGUI _staminaValueLabel;
        [SerializeField] private TextMeshProUGUI _strengthValueLabel;
        [SerializeField] private TextMeshProUGUI _sanityValueLabel;
        [SerializeField] private TextMeshProUGUI _radiationValueLabel;
        [SerializeField] private TextMeshProUGUI _hungerValueLabel;
        [SerializeField] private TextMeshProUGUI _thirstValueLabel;
        [SerializeField] private TextMeshProUGUI _alcoholValueLabel;
        [SerializeField] private TextMeshProUGUI _drugValueLabel;

        private Player.Player _player;

        private void Awake()
        {
            var holder = GetComponentInParent<PlayerHUDVariableHolder>();
            _player = holder.Player;
        }

        private void Update()
        {
            _staminaValueLabel.SetText(_player.Stats.Stamina.ToString(StringFormatType));
            _strengthValueLabel.SetText(_player.Stats.Strength.ToString(StringFormatType));
            _sanityValueLabel.SetText(_player.Stats.Sanity.ToString(StringFormatType));
            _radiationValueLabel.SetText(_player.Stats.Radiation.ToString(StringFormatType));
            _hungerValueLabel.SetText(_player.Stats.Hunger.ToString(StringFormatType));
            _thirstValueLabel.SetText(_player.Stats.Thirst.ToString(StringFormatType));
            _alcoholValueLabel.SetText(_player.Stats.Alcohol.ToString(StringFormatType));
            _drugValueLabel.SetText(_player.Stats.Drug.ToString(StringFormatType));
        }
    }
}