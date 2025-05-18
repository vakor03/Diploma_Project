using System;
using _Project.Features.StatsModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.DamageModule {
    public class SimpleMonoDamageable : MonoDamageable {
        [SerializeField] private HurtBox[] _hurtBoxes;
        private IStatService<EntityStats> _stats;
        
        [Inject]
        private void InjectDependencies(IStatService<EntityStats> stats) =>
            _stats = stats;

        private float Health { get => _stats[EntityStats.CurrentHealth]; set => _stats[EntityStats.CurrentHealth] = value; }

        private void OnEnable() {
            _stats[EntityStats.CurrentHealth] = _stats[EntityStats.MaxHealth];
            foreach (HurtBox hurtBox in _hurtBoxes)
                hurtBox.Damageable = this;
        }

        public override event Action<float> OnTakeDamage;
        public override event Action OnDeath;
        public override event Action OnAfterDeath;

        public override void TakeDamage(float damage) {
            if (Health <= 0)
                return;

            Health -= damage;
            OnTakeDamage?.Invoke(damage);

            if (Health <= 0) {
                Health = 0;
                OnDeath?.Invoke();
                OnAfterDeath?.Invoke();
            }
        }
    }
}