using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Features.Enemy {
    public class EntityHealthController : MonoBehaviour {
        [Header("Health Settings")]
        [SerializeField] private EntityHealthData _healthData;

        [Header("Events")]
        public UnityEvent<HealthChangeArgs> OnHealthChanged;

        public UnityEvent<DamageArgs> OnTakeDamage;
        public UnityEvent<HealArgs> OnHealed;
        public UnityEvent OnDeath;

        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsInvulnerable { get; private set; }

        private float _invulnerabilityEndTime;

        public event Action<HealthChangeArgs> HealthChanged;
        public event Action<DamageArgs> DamageReceived;
        public event Action<HealArgs> Healed;
        public event Action<DeathArgs> Death;

        public void Setup(EntityHealthData healthData) {
            _healthData = healthData;
            MaxHealth = _healthData.maxHealth;
            CurrentHealth = _healthData.startingHealth > 0 ? _healthData.startingHealth : MaxHealth;
            IsDead = false;
            IsInvulnerable = false;
        }

        private void Awake() {
            if (_healthData != null) {
                Setup(_healthData);
            }
        }

        private void Update() {
            UpdateInvulnerability();
        }

        private void UpdateInvulnerability() {
            if (IsInvulnerable && Time.time >= _invulnerabilityEndTime) {
                IsInvulnerable = false;
            }
        }

        public void TakeDamage(float damage, DamageType damageType = DamageType.Physical, Transform damageSource = null) {
            if (IsDead || IsInvulnerable)
                return;

            // Apply resistances
            damage = ApplyResistances(damage, damageType);

            // Apply damage
            float previousHealth = CurrentHealth;
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

            // Create damage args
            DamageArgs damageArgs = new DamageArgs {
                Damage = damage,
                DamageType = damageType,
                DamageSource = damageSource,
                RemainingHealth = CurrentHealth,
                PreviousHealth = previousHealth
            };

            // Invoke events
            OnTakeDamage?.Invoke(damageArgs);
            DamageReceived?.Invoke(damageArgs);

            // Apply invulnerability if configured
            if (_healthData.invulnerabilityTime > 0) {
                MakeInvulnerable(_healthData.invulnerabilityTime);
            }

            // Check for death
            if (CurrentHealth <= 0) {
                Die(damageSource);
            }
            else {
                NotifyHealthChanged();
            }
        }

        public void Heal(float amount, HealType healType = HealType.Direct, Transform healSource = null) {
            if (IsDead)
                return;

            float previousHealth = CurrentHealth;
            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);

            // Create heal args
            HealArgs healArgs = new HealArgs {
                HealAmount = amount,
                HealType = healType,
                HealSource = healSource,
                CurrentHealth = CurrentHealth,
                PreviousHealth = previousHealth
            };

            // Invoke events
            OnHealed?.Invoke(healArgs);
            Healed?.Invoke(healArgs);

            NotifyHealthChanged();
        }

        [Button]
        public void InstantDeath(Transform killer = null) {
            if (IsDead)
                return;

            CurrentHealth = 0;
            Die(killer, isInstantDeath: true);
        }

        private void Die(Transform killer = null, bool isInstantDeath = false) {
            if (IsDead)
                return;

            IsDead = true;

            DeathArgs deathArgs = new DeathArgs {
                Killer = killer,
                IsInstantDeath = isInstantDeath,
                EntityTransform = transform
            };

            NotifyHealthChanged();

            OnDeath?.Invoke();
            Death?.Invoke(deathArgs);
        }

        public void MakeInvulnerable(float duration) {
            IsInvulnerable = true;
            _invulnerabilityEndTime = Time.time + duration;
        }

        public void FullHeal() {
            Heal(MaxHealth - CurrentHealth);
        }

        public void SetMaxHealth(float newMaxHealth, bool healToFull = false) {
            MaxHealth = newMaxHealth;

            if (healToFull) {
                CurrentHealth = MaxHealth;
            }
            else {
                CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
            }

            NotifyHealthChanged();
        }

        private float ApplyResistances(float damage, DamageType damageType) {
            if (_healthData.damageResistances != null && _healthData.damageResistances.TryGetValue(damageType, out float resistance)) {
                return damage * (1 - resistance);
            }

            return damage;
        }

        private void NotifyHealthChanged() {
            HealthChangeArgs args = new HealthChangeArgs {
                CurrentHealth = CurrentHealth,
                MaxHealth = MaxHealth,
                HealthPercentage = MaxHealth > 0 ? CurrentHealth / MaxHealth : 0
            };

            OnHealthChanged?.Invoke(args);
            HealthChanged?.Invoke(args);
        }
    }
}