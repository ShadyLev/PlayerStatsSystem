using Code.PlayerStats;
using ScriptableObjects;
using UnityEngine;
using VisitorPattern;

namespace Code.Player
{
    public abstract class PlayerEntity : MonoBehaviour, IVisitable
    {
        [SerializeField] BaseStats baseStats;
        public Stats Stats { get; private set; }
        public Inventory.Inventory Inventory { get; private set; }

        public void Awake()
        {
            Stats = new Stats(new StatsMediator(), baseStats);
            Inventory = new Inventory.Inventory();
            Inventory.Init();
            Stats.Init();
        }

        public void Update()
        {
            Stats.Mediator.Update(Time.deltaTime);
            Stats.Update();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}