using System;
using Code.Utility;

namespace Code.PlayerStats
{
    public enum OperationType
    {
        Add,
        Multiply,
    }

    public enum ModifierType
    {
        ValueModifier,
        ValueChange,
        CoTModifier,
        CoTChange
    }
    
    [Serializable]
    public struct StatEffect
    {
        public StatType statType;
        public ModifierType modifierType;
        public OperationType operationType;
        public float value;
        public int durationSeconds;
    }
    
    public class BasicStatModifier : StatModifier
    {
        readonly StatType _statType;
        readonly ModifierType _modifierType;
        private readonly Func<float, float> _operation;
        
        public BasicStatModifier(StatType statType, ModifierType modifierType, float duration, Func<float, float> operation) : base(modifierType, duration)
        {
            _statType = statType;
            _operation = operation;
            _modifierType = modifierType;
        }

        public override void Handle(object sender, Query query)
        {
            if (query.StatType == _statType && query.ModifierType == _modifierType)
            {
                query.Value = _operation(query.Value);
            }
        }
    }
    
    public abstract class StatModifier : IDisposable
    {
        public bool MarkedForRemoval { get; set; }
        public event Action<StatModifier> OnDispose = delegate {};
        
        readonly CountdownTimer _timer;

        protected StatModifier(ModifierType type, float duration)
        {
            if (duration <= 0)
            {
                return;
            }
            
            _timer = new CountdownTimer(duration);
            _timer.OnTimerStop += () => MarkedForRemoval = true;
            _timer.Start();
        }

        public void Update(float deltaTime)
        {
            _timer?.Tick(deltaTime);
        }
        
        public abstract void Handle(object sender, Query query);

        public void Dispose()
        {
            MarkedForRemoval = true;
            OnDispose.Invoke(this);
        }
    }
}