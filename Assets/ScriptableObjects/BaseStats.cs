using System;
using System.Collections.Generic;
using Code.PlayerStats;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BaseStats", menuName = "ScriptableObjects/BaseStats", order = 1)]
    public class BaseStats : ScriptableObject
    {
        public List<Stat> Stats = new();
    }
    
    [Serializable]
    public class Stat
    {
        public StatType Type;
        public float Value;
        public float MinValue;
        public float MaxValue;
        public float ChangePerSecond;
        public float ChangePerSecondMin;
        public float ChangePerSecondMax;
    }
}