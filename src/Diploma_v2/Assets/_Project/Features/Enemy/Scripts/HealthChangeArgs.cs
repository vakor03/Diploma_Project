using System;

namespace _Project.Features.Enemy {
    [Serializable]
    public class HealthChangeArgs
    {
        public float CurrentHealth;
        public float MaxHealth;
        public float HealthPercentage;
    }
}