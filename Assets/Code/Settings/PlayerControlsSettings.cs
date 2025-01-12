using Code.Singletons;
using UnityEngine;

namespace Shady.Game.PlayerSettings
{
    public class PlayerControlsSettings : PersistentSingleton<PlayerControlsSettings>
    {
        public float MouseSensitivity
        {
            get => _mouseSensitivity;
        }
        
        [SerializeField] 
        private float defaultMouseSensitivity = 0.2f;
        private const string MouseSensitivityKey = "MouseSensitivity";
        private float _mouseSensitivity;
        
        void Awake()
        {
            base.Awake();
            LoadControlSettings();
        }

        void LoadControlSettings()
        {
            _mouseSensitivity = defaultMouseSensitivity;
            //_mouseSensitivity = PlayerPrefs.HasKey(MouseSensitivityKey) ? PlayerPrefs.GetFloat(MouseSensitivityKey) : 2f;
        }
    }
}