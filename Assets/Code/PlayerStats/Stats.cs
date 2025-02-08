using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;

namespace Code.PlayerStats
{
    public enum StatType
    {
        Stamina,
        Strength,
        Sanity,
        Radiation,
        Hunger,
        Thirst,
        Alcohol,
        Drug
    }
    
    public class Stats
    {
        public StatsMediator Mediator => _mediator;
        
        // Set By constructor only
        private readonly BaseStats _baseStats;
        private readonly StatsMediator _mediator;
        private readonly Dictionary<StatType, Stat> _playerStats = new();
        private readonly Dictionary<ModifierType, Dictionary<StatType, Query>> _queries = new();
        
        public Stats(StatsMediator mediator, BaseStats baseStats)
        {
            _mediator = mediator;
            _baseStats = baseStats;
        }
        
        public void Init()
        {
            // Initialize values from base stats
            InitializeQueries();
        }
        
        public void Update()
        {
            UpdateAllStats();
        }

        public void ModifyStat(StatType statType, float value)
        {
            var stat = _playerStats[statType];
            stat.Value += value;
        }
        
        public void ModifyStatCoT(StatType statType, float value)
        {
            var stat = _playerStats[statType];
            stat.ChangePerSecond += value;
        }
        
        public float GetStatValueFromType(StatType statType)
        {
            return GetStat(statType, ModifierType.ValueModifier);
        }
        
        public float GetStatValueFromType(StatType statType, out float maxValue)
        {
            var stat = _playerStats[statType];
            maxValue = stat.MaxValue;
            return GetStat(statType, ModifierType.ValueModifier);
        }

        private void UpdateAllStats()
        {
            foreach (var kvp in _playerStats)
            {
                UpdateStat(kvp.Value, Time.deltaTime);
            }
        }
        
        private void UpdateStat(Stat stat, float deltaTime)
        {
            var statChange = GetStat(stat.Type, ModifierType.CoTModifier) * deltaTime;
            stat.Value = math.clamp(stat.Value + statChange, stat.MinValue, stat.MaxValue);
        }

        float GetStat(StatType statType, ModifierType modifierType)
        {
            var stat = _playerStats[statType];
            var query = _queries[modifierType][statType];
            var statValue = modifierType == ModifierType.ValueModifier ? stat.Value : stat.ChangePerSecond;
            query.Value = statValue;
            _mediator.PerformQuery(this, query);

            // Ideally each value type (normal/cot) should be its own class/struct
            // so we don't have to do this if statement but just get minmax from stat directly
            if (modifierType == ModifierType.ValueModifier)
            {
                query.Value = math.clamp(query.Value, stat.MinValue, stat.MaxValue);
            }
            else
            {
                query.Value = math.clamp(query.Value, stat.ChangePerSecondMin, stat.ChangePerSecondMax);
            }
            
            return query.Value;
        }

        private bool InitializeQueries()
        {
            InitQueryDictionary();
            
            var stats = _baseStats.Stats;
            foreach (var stat in stats)
            {
                if (_playerStats.TryAdd(stat.Type, stat) == false)
                {
                    UnityEngine.Debug.LogError($"Duplicate Stat of type {stat.Type} found.");
                    continue;
                }

                var query = new Query(stat.Type, ModifierType.ValueModifier, stat.Value);
                _queries[ModifierType.ValueModifier].Add(stat.Type, query);
                
                var cotQuery = new Query(stat.Type, ModifierType.CoTModifier, stat.ChangePerSecond);
                _queries[ModifierType.CoTModifier].Add(stat.Type, cotQuery);
            }
            
            return true;
        }

        void InitQueryDictionary()
        {
            var enums = Enum.GetValues(typeof(ModifierType)).Cast<ModifierType>();
            foreach (var modifierType in enums)
            {
                _queries.Add(modifierType, new Dictionary<StatType, Query>());
            }
        }
    }
}