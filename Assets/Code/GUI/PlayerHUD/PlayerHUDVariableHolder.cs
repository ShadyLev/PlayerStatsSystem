using Code.Player.Interaction;
using UnityEngine;

namespace Code.GUI.PlayerHUD
{
    /// <summary>
    /// Horrible name I know but this class holds references that are used by child canvases for example "Player".
    /// This way constant references don't need to be individually assigned on child objects.
    /// TODO: In the future I'd like to Inject these variables via dependency injection instead of GetComponentInParent
    /// </summary>
    public class PlayerHUDVariableHolder : MonoBehaviour
    {
        [SerializeField] private Player.Player _player;
        [SerializeField] private HotbarInteraction _playerHotbar;
        
        public Player.Player Player => _player;
        public HotbarInteraction PlayerHotbar => _playerHotbar;
    }
}