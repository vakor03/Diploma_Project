using System;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public interface IEntityHealth {
        public float MaxHealth { get; }
        public float CurrentHealth { get; }
        
        public event Action OnHealthChanged;

        public float ResetHealth();
    }
}