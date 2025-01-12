using System;
using System.Collections.Generic;
using Code.Player;
using Code.PlayerStats;
using UnityEngine;

namespace Code.Items.ItemsWithModifiers
{
    public abstract class StatModifierItem : Item
    {
        [Header("Effect")]
        // This can easly be a struct, make it a list so we can apply multiple effects
        [SerializeField] private List<StatEffect> effects = new();
        
        /// <summary>
        /// Move this and the other thing to an abstract ItemWithStatModifier class
        /// </summary>
        /// <param name="playerEntity"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected void ApplyPickupEffect(PlayerEntity playerEntity)
        {
            foreach (var effect in effects)
            {
                // Um gotta do something about this allocation
                var temp = effect;

                switch (temp.modifierType)
                {
                    case ModifierType.CoTModifier:
                    case ModifierType.ValueModifier:
                        ApplyValueStatModifier(playerEntity, temp);
                        break;
                    case ModifierType.ValueChange:
                        playerEntity.Stats.ModifyStat(temp.statType, temp.value);
                        break;
                    case ModifierType.CoTChange:
                        playerEntity.Stats.ModifyStatCoT(temp.statType, temp.value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        void ApplyValueStatModifier(PlayerEntity entity, StatEffect effect)
        {
            StatModifier modifier;
            switch (effect.operationType)
            {
                case OperationType.Add:
                    modifier = new BasicStatModifier(effect.statType, effect.modifierType, effect.durationSeconds, v => v + effect.value);
                    break;
                case OperationType.Multiply:
                    modifier = new BasicStatModifier(effect.statType, effect.modifierType, effect.durationSeconds, v => v * effect.value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            entity.Stats.Mediator.AddModifier(modifier);
        }

        public abstract override void OnUse(PlayerEntity entity);
    }
}