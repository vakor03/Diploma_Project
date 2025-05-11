using System;
using _Project.Scripts.Infrastructure;

namespace _Project.Features.PlayerModule {
    public class PlayerHealthModel : IModel {
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        
        public float HealthPercentage => MaxHealth > 0 ? (float)CurrentHealth / MaxHealth : 0f;
        
        public event Action<int> OnCurrentHealthChanged;
        public event Action<int> OnMaxHealthChanged;
        
        public void SetCurrentHealth(int health) {
            CurrentHealth = health;
            OnCurrentHealthChanged?.Invoke(health);
        }
        
        public void SetMaxHealth(int health) {
            MaxHealth = health;
            OnMaxHealthChanged?.Invoke(health);
        }
    }
}