using System;
using System.Collections.Generic;
using Code.PlayerStats;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.PlayerHUD
{
    public class PlayerStatusBarDisplay : MonoBehaviour
    {
        [Serializable]
        struct PlayerStatusGUIData
        {
            public StatType statType;
            public Image statusBarImage;

            public bool inverse;
            
            // Change based on something
            public Color statusBarColor;
            public Color statusBarWarningColor;
            public Color statusBarCriticalColor;

            [Range(0, 1)]
            public float barWarningStage;
            
            [Range(0, 1)]
            public float barCriticalStage;

            public PlayerStatusGUIData(StatType statType, Image statusBarImage)
            {
                this.statType = statType;
                this.statusBarImage = statusBarImage;
                statusBarColor = Color.white;
                statusBarWarningColor = Color.yellow;
                statusBarCriticalColor = Color.red;
                barWarningStage = 0.7f;
                barCriticalStage = 0.9f;
                inverse = false;
            }
        }
        
        [SerializeField] private List<PlayerStatusGUIData> _playerSatusGUIDatas;
        private Stats _playerStats;

        private void Start()
        {
            var holder = GetComponentInParent<PlayerHUDVariableHolder>();
            _playerStats = holder.Player.Stats;
        }

        private void Update()
        {
            foreach (var guiData in _playerSatusGUIDatas)
            {
                var statValue = _playerStats.GetStatValueFromType(guiData.statType, out var maxValue);
                var fillAmount = statValue / maxValue;
                
                var isWarning = fillAmount >= guiData.barWarningStage;
                var isCritical = fillAmount >= guiData.barCriticalStage;

                if (guiData.inverse)
                {
                    isWarning = !isWarning;
                    isCritical = !isCritical;
                }
                
                var barColor = guiData.statusBarColor;
                if (isWarning && isCritical == false)
                {
                    barColor = guiData.statusBarWarningColor;
                }else if (isCritical)
                {
                    barColor = guiData.statusBarCriticalColor;
                }
                
                guiData.statusBarImage.color = barColor;
                guiData.statusBarImage.fillAmount = statValue / maxValue;
            }
        }
    }
}