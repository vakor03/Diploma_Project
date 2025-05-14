using System;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public interface IDamageable {
        public event Action<float> OnTakeDamage;
        public event Action OnDeath;

        public void TakeDamage(float damage);
    }
}