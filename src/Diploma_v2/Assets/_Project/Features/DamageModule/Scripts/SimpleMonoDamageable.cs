using System;
using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class SimpleMonoDamageable : MonoDamageable {
        [SerializeField] private HurtBox[] _hurtBoxes;
        [SerializeField] private float _health = 100f;

        private void OnEnable() {
            foreach (HurtBox hurtBox in _hurtBoxes)
                hurtBox.Damageable = this;
        }

        public override event Action<float> OnTakeDamage;
        public override event Action OnDeath;
        public override event Action OnAfterDeath;

        public override void TakeDamage(float damage) {
            if (_health <= 0)
                return;

            _health -= damage;
            OnTakeDamage?.Invoke(damage);

            if (_health <= 0) {
                _health = 0;
                OnDeath?.Invoke();
                OnAfterDeath?.Invoke();
            }
        }
    }
}