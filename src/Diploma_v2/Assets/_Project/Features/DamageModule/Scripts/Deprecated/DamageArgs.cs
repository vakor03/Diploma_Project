using System;
using UnityEngine;

namespace _Project.Features.Enemy {
    [Serializable]
    public class DamageArgs
    {
        public float Damage;
        public DamageType DamageType;
        public Transform DamageSource;
        public float RemainingHealth;
        public float PreviousHealth;
    }
}