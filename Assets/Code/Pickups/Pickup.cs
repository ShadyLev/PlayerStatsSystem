using System;
using Code.Player;
using Code.PlayerStats;
using UnityEngine;
using VisitorPattern;

namespace Code.Pickups
{
    public class GenericPickup : Pickup
    {
        public enum OperationType
        {
            Add,
            Multiply,
        }
        
        [SerializeField] StatType statType;
        [SerializeField] OperationType operationType;
        [SerializeField] float value;
        [SerializeField] private int duration;
        [SerializeField] private ModifierType modifierType;

        protected override void ApplyPickupEffect(PlayerEntity playerEntity)
        {
            StatModifier modifier;
            switch (operationType)
            {
                case OperationType.Add:
                    modifier = new BasicStatModifier(statType, modifierType, duration, v => value += v);
                    break;
                case OperationType.Multiply:
                    modifier = new BasicStatModifier(statType, modifierType, duration, v => value *= v);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            playerEntity.Stats.Mediator.AddModifier(modifier);
        }
}
    
    public abstract class Pickup : MonoBehaviour, IVisitor
    {
        protected abstract void ApplyPickupEffect(PlayerEntity playerEntity);

        public void Visit<T>(T visitable) where T : Component, IVisitable
        {
            if (visitable is PlayerEntity entity)
            {
                ApplyPickupEffect(entity);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            other.GetComponent<IVisitable>()?.Accept(this);
            Destroy(gameObject);
        }
    }
}