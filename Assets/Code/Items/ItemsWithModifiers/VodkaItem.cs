using Code.Player;

namespace Code.Items.ItemsWithModifiers
{
    public class VodkaItem : StatModifierItem
    {
        public override void OnUse(PlayerEntity entity)
        {
            UnityEngine.Debug.Log("USE VODKA ITEM");
            // Apply effects
            ApplyPickupEffect(entity);

            // Drop empty bottle
        }
    }
}