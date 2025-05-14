using System;
using _Project.Features.StatsModule;
using _Project.Scripts.Core.HUB;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public class EntityHealth : MonoBehaviour, IEntityHealth, IDamageable, IHealable {
        [Inject] private IStatService<EntityStats> _stats;

        public event Action<float> OnTakeDamage;
        public event Action OnDeath;

        public void TakeDamage(float damage) {
            if (CurrentHealth <= 0)
                return;

            _stats[EntityStats.CurrentHealth] = Mathf.Max(CurrentHealth - damage, 0);

            OnTakeDamage?.Invoke(damage);
            OnHealthChanged?.Invoke();

            if (CurrentHealth <= 0)
                OnDeath?.Invoke();
        }

        public float MaxHealth => _stats[EntityStats.MaxHealth];
        public float CurrentHealth => _stats[EntityStats.CurrentHealth];
        public event Action OnHealthChanged;

        public float ResetHealth() {
            _stats[EntityStats.CurrentHealth] = MaxHealth;
            OnHealthChanged?.Invoke();
            return CurrentHealth;
        }

        public event Action<float> OnHeal;

        public void Heal(float healAmount) {
            if (CurrentHealth <= 0)
                return;

            _stats[EntityStats.CurrentHealth] = Mathf.Min(CurrentHealth, MaxHealth);

            OnHealthChanged?.Invoke();
            OnHeal?.Invoke(healAmount);
        }
    }
}