using System;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public interface IDamageable {
        public event Action<float> OnTakeDamage;
        public event Action OnDeath;
        public event Action OnAfterDeath;

        public void TakeDamage(float damage);
    }
}