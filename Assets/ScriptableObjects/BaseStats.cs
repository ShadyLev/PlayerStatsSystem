using System;
using Code.PlayerStats;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BaseStats", menuName = "ScriptableObjects/BaseStats", order = 1)]
    public class BaseStats : ScriptableObject
    {
        public Stat Stamina;
        public Stat Strength;
        public Stat Sanity;
        public Stat Radiation;
        public Stat Hunger;
        public Stat Thirst;
        public Stat Alcohol;
        public Stat Drug;
    }
    
    [Serializable]
    public struct Stat
    {
        public StatType Type;
        public float Value;
        public float MaxValue;
        public float ChangePerSecond;
    }
}