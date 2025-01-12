using System;
using System.Collections.Generic;

namespace Code.PlayerStats
{
    public class StatsMediator
    {
        readonly LinkedList<StatModifier> _modifiers = new();

        private EventHandler<Query> _statChangeQueries;
        public void PerformQuery(object sender, Query query)
        {
            _statChangeQueries?.Invoke(sender, query);
            
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.AddLast(modifier);
            _statChangeQueries += modifier.Handle;
            
            modifier.OnDispose += _ =>
            {
                _modifiers.Remove(modifier);
                _statChangeQueries -= modifier.Handle;
            };
        }

        public void Update(float deltaTime)
        {
            // Update all modifiers with deltaTime;
            var node = _modifiers.First;
            while (node != null)
            {
                var modifier = node.Value;
                modifier.Update(deltaTime);
                node = node.Next;
            }
            
            // Dispose of any modifiers that are finished
            node = _modifiers.First;
            while (node != null)
            {
                var nextNode = node.Next;

                if (node.Value.MarkedForRemoval)
                { 
                    node.Value.Dispose();
                }
                
                node = nextNode;
            }
        }
    }

    public class Query
    {
        public readonly StatType StatType;
        public readonly ModifierType ModifierType;
        public float Value;

        public Query(StatType statType, ModifierType modifierType, float value)
        {
            StatType = statType;
            Value = value;
            ModifierType = modifierType;
        }
    }
}