using System;
using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class SimpleMonoDamageable : MonoBehaviour, IDamageable {
        [SerializeField] private HurtBox[] _hurtBoxes;
        [SerializeField] private float _health = 100f;

        private void OnEnable() {
            foreach (HurtBox hurtBox in _hurtBoxes)
                hurtBox.Damageable = this;
        }

        public event Action<float> OnTakeDamage;
        public event Action OnDeath;
        public event Action OnAfterDeath;

        public void TakeDamage(float damage) {
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