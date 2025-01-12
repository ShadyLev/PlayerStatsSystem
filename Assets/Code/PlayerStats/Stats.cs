using System;
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
        readonly BaseStats baseStats;
        private readonly StatsMediator _mediator;
        
        // Private stats, based on baseStat SO
        private Stat _staminaStat;
        private Stat _strengthStat;
        private Stat _sanityStat;
        private Stat _radiationStat;
        private Stat _hungerStat;
        private Stat _thirstStat;
        private Stat _alcoholStat;
        private Stat _drugStat;

        #region Query Classes

        // Private Query Classes
        private Query _staminaQuery;
        private Query _strengthQuery;
        private Query _sanityQuery;
        private Query _radiationQuery;
        private Query _hungerQuery;
        private Query _thirstQuery;
        private Query _alcoholQuery;
        private Query _drugQuery;
        
        private Query _staminaCoTQuery;
        private Query _strengthCoTQuery;
        private Query _sanityCoTQuery;
        private Query _radiationCoTQuery;
        private Query _hungerCoTQuery;
        private Query _thirstCoTQuery;
        private Query _alcoholCoTQuery;
        private Query _drugCoTQuery; 

        #endregion
        
        public Stats(StatsMediator mediator, BaseStats baseStats)
        {
            _mediator = mediator;
            this.baseStats = baseStats;
        }
        
        #region Public Properties

        public float Stamina
        {
            get
            {
                ValidateQueryClass(ref _staminaQuery, StatType.Stamina, ModifierType.ValueModifier, _staminaStat.Value);
                _mediator.PerformQuery(this, _staminaQuery);
                _staminaQuery.Value = math.clamp(_staminaQuery.Value, 0, _staminaStat.MaxValue);
                return _staminaQuery.Value;
            }
        }
        
        public float Strength
        {
            get
            {
                ValidateQueryClass(ref _strengthQuery, StatType.Strength, ModifierType.ValueModifier, _strengthStat.Value);
                _mediator.PerformQuery(this, _strengthQuery);
                _strengthQuery.Value = math.clamp(_strengthQuery.Value, 0, _strengthStat.MaxValue);
                return _strengthQuery.Value;
            }
        }
        
        public float Sanity
        {
            get
            {
                ValidateQueryClass(ref _sanityQuery, StatType.Sanity, ModifierType.ValueModifier, _sanityStat.Value);
                _mediator.PerformQuery(this, _sanityQuery);
                _sanityQuery.Value = math.clamp(_sanityQuery.Value, 0, _sanityStat.MaxValue);
                return _sanityQuery.Value;
            }
        }
        
        public float Radiation
        {
            get
            {
                ValidateQueryClass(ref _radiationQuery, StatType.Radiation, ModifierType.ValueModifier, _radiationStat.Value);
                _mediator.PerformQuery(this, _radiationQuery);
                _radiationQuery.Value = math.clamp(_radiationQuery.Value, 0, _radiationStat.MaxValue);
                return _radiationQuery.Value;
            }
        }
        
        public float Hunger
        {
            get
            {
                ValidateQueryClass(ref _hungerQuery, StatType.Hunger, ModifierType.ValueModifier, _hungerStat.Value);
                _mediator.PerformQuery(this, _hungerQuery);
                _hungerQuery.Value = math.clamp(_hungerQuery.Value, 0, _hungerStat.MaxValue);
                return _hungerQuery.Value;
            }
        }
        
        public float Thirst
        {
            get
            {
                ValidateQueryClass(ref _thirstQuery, StatType.Thirst, ModifierType.ValueModifier, _thirstStat.Value);
                _mediator.PerformQuery(this, _thirstQuery);
                _thirstQuery.Value = math.clamp(_thirstQuery.Value, 0, _thirstStat.MaxValue);
                return _thirstQuery.Value;
            }
        }
        
        public float Alcohol
        {
            get
            {
                ValidateQueryClass(ref _alcoholQuery, StatType.Alcohol, ModifierType.ValueModifier, _alcoholStat.Value);
                _mediator.PerformQuery(this, _alcoholQuery);
                _alcoholQuery.Value = math.clamp(_alcoholQuery.Value, 0, _alcoholStat.MaxValue);
                return _alcoholQuery.Value;
            }
        }
        
        public float Drug
        {
            get
            {
                ValidateQueryClass(ref _drugQuery, StatType.Drug, ModifierType.ValueModifier, _drugStat.Value);
                _mediator.PerformQuery(this, _drugQuery);
                _drugQuery.Value = math.clamp(_drugQuery.Value, 0, _drugStat.MaxValue);
                return _drugQuery.Value;
            }
        }

        public float StaminaCoT
        {
            get
            {
                ValidateQueryClass(ref _staminaCoTQuery, StatType.Stamina, ModifierType.CoTModifier, _staminaStat.ChangePerSecond);
                _mediator.PerformQuery(this, _staminaCoTQuery);
                return _staminaCoTQuery.Value;
            }
        }
        
        public float StrengthCoT
        {
            get
            {
                ValidateQueryClass(ref _strengthCoTQuery, StatType.Strength, ModifierType.CoTModifier, _strengthStat.ChangePerSecond);
                _mediator.PerformQuery(this, _strengthCoTQuery);
                return _strengthCoTQuery.Value;
            }
        }
        
        public float SanityCoT
        {
            get
            {
                ValidateQueryClass(ref _sanityCoTQuery, StatType.Sanity, ModifierType.CoTModifier, _sanityStat.ChangePerSecond);
                _mediator.PerformQuery(this, _sanityCoTQuery);
                return _sanityCoTQuery.Value;
            }
        }
        
        public float RadiationCoT
        {
            get
            {
                ValidateQueryClass(ref _radiationCoTQuery, StatType.Radiation, ModifierType.CoTModifier, _radiationStat.ChangePerSecond);
                _mediator.PerformQuery(this, _radiationCoTQuery);
                return _radiationCoTQuery.Value;
            }
        }
        
        public float HungerCoT
        {
            get
            {
                ValidateQueryClass(ref _hungerCoTQuery, StatType.Hunger, ModifierType.CoTModifier, _hungerStat.ChangePerSecond);
                _mediator.PerformQuery(this, _hungerCoTQuery);
                return _hungerCoTQuery.Value;
            }
        }
        
        public float ThirstCoT
        {
            get
            {
                ValidateQueryClass(ref _thirstCoTQuery, StatType.Thirst, ModifierType.CoTModifier, _thirstStat.ChangePerSecond);
                _mediator.PerformQuery(this, _thirstCoTQuery);
                return _thirstCoTQuery.Value;
            }
        }
        
        public float AlcoholCoT
        {
            get
            {
                ValidateQueryClass(ref _alcoholCoTQuery, StatType.Alcohol, ModifierType.CoTModifier, _alcoholStat.ChangePerSecond);
                _mediator.PerformQuery(this, _alcoholCoTQuery);
                return _alcoholCoTQuery.Value;
            }
        }
        
        public float DrugCoT
        {
            get
            {
                ValidateQueryClass(ref _drugCoTQuery, StatType.Drug, ModifierType.CoTModifier, _drugStat.ChangePerSecond);
                _mediator.PerformQuery(this, _drugCoTQuery);
                return _drugCoTQuery.Value;
            }
        }
        
        #endregion
        
        public void Init()
        {
            // Initialize values from base stats
            _staminaStat = baseStats.Stamina;
            _strengthStat = baseStats.Strength;
            _sanityStat = baseStats.Sanity;
            _radiationStat = baseStats.Radiation;
            _hungerStat = baseStats.Hunger;
            _thirstStat = baseStats.Thirst;
            _alcoholStat = baseStats.Alcohol;
            _drugStat = baseStats.Drug;
        }
        
        public void Update()
        {
            UpdateAllStats();
        }

        public void ModifyStat(StatType statType, float value)
        {
            switch (statType)
            {
                case StatType.Stamina:
                    _staminaStat.Value += value;
                    break;
                case StatType.Strength:
                    _strengthStat.Value += value;
                    break;
                case StatType.Sanity:
                    _sanityStat.Value += value;
                    break;
                case StatType.Radiation:
                    _radiationStat.Value += value;
                    break;
                case StatType.Hunger:
                    _hungerStat.Value += value;
                    break;
                case StatType.Thirst:
                    _thirstStat.Value += value;
                    break;
                case StatType.Alcohol:
                    _alcoholStat.Value += value;
                    break;
                case StatType.Drug:
                    _hungerStat.Value += value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }
        
        public void ModifyStatCoT(StatType statType, float value)
        {
            switch (statType)
            {
                case StatType.Stamina:
                    _staminaStat.ChangePerSecond += value;
                    break;
                case StatType.Strength:
                    _strengthStat.ChangePerSecond += value;
                    break;
                case StatType.Sanity:
                    _sanityStat.ChangePerSecond += value;
                    break;
                case StatType.Radiation:
                    _radiationStat.ChangePerSecond += value;
                    break;
                case StatType.Hunger:
                    _hungerStat.ChangePerSecond += value;
                    break;
                case StatType.Thirst:
                    _thirstStat.ChangePerSecond += value;
                    break;
                case StatType.Alcohol:
                    _alcoholStat.ChangePerSecond += value;
                    break;
                case StatType.Drug:
                    _hungerStat.ChangePerSecond += value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }
        
        public float GetStatValueFromType(StatType statType)
        {
            return statType switch
            {
                StatType.Stamina => Stamina,
                StatType.Strength => Strength,
                StatType.Sanity => Sanity,
                StatType.Radiation => Radiation,
                StatType.Hunger => Hunger,
                StatType.Thirst => Thirst,
                StatType.Alcohol => Alcohol,
                StatType.Drug => Drug,
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
        }
        
        public float GetStatValueFromType(StatType statType, out float maxValue)
        {
            switch (statType)
            {
                case StatType.Stamina:
                    maxValue = _staminaStat.MaxValue;
                    return Stamina;
                case StatType.Strength:
                    maxValue = _strengthStat.MaxValue;
                    return Strength;
                case StatType.Sanity:
                    maxValue = _sanityStat.MaxValue;
                    return Sanity;
                case StatType.Radiation:
                    maxValue = _radiationStat.MaxValue;
                    return Radiation;
                case StatType.Hunger:
                    maxValue = _hungerStat.MaxValue;
                    return Hunger;
                case StatType.Thirst:
                    maxValue = _thirstStat.MaxValue;
                    return Thirst;
                case StatType.Alcohol:
                    maxValue = _alcoholStat.MaxValue;
                    return Alcohol;
                case StatType.Drug:
                    maxValue = _drugStat.MaxValue;
                    return Drug;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        private void UpdateAllStats()
        {
            UpdateStat(ref _staminaStat, Time.deltaTime);
            UpdateStat(ref _strengthStat, Time.deltaTime);
            UpdateStat(ref _sanityStat, Time.deltaTime);
            UpdateStat(ref _radiationStat, Time.deltaTime);
            UpdateStat(ref _hungerStat, Time.deltaTime);
            UpdateStat(ref _thirstStat, Time.deltaTime);
            UpdateStat(ref _alcoholStat, Time.deltaTime);
            UpdateStat(ref _drugStat, Time.deltaTime);
        }
        
        private void UpdateStat(ref Stat stat, float deltaTime)
        {
            var statChange = GetStatCoTValueFromType(stat.Type) * deltaTime;
            stat.Value = math.clamp(stat.Value + statChange, 0, stat.MaxValue);
        }

        float GetStatCoTValueFromType(StatType statType)
        {
            return statType switch
            {
                StatType.Stamina => StaminaCoT,
                StatType.Strength => StrengthCoT,
                StatType.Sanity => SanityCoT,
                StatType.Radiation => RadiationCoT,
                StatType.Hunger => HungerCoT,
                StatType.Thirst => ThirstCoT,
                StatType.Alcohol => AlcoholCoT,
                StatType.Drug => DrugCoT,
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
        }
        
        //<summary>
        // (Use ValidateQueryClass to update and avoid reallocating a new class per query)
        // Query classes are reused
        // Because we calculate modifiers by using Unity EventHandler each query must be a class as such for Handle methods to be able to modify the data
        // You could create a custom invoke to pass in a struct but that is probably not worth it.
        //</summary>
        private Query ValidateQueryClass(ref Query query, StatType statType, ModifierType modifierType, float value)
        {
            if (query == null)
            {
                query = new Query(statType, modifierType, value);
            }
            else
            {
                query.Value = value;
            }

            return query;
        }
    }
}