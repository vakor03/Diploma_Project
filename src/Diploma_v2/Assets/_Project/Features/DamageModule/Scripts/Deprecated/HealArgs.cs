using System;
using UnityEngine;

namespace _Project.Features.Enemy {
    [Serializable]
    public class HealArgs
    {
        public float HealAmount;
        public HealType HealType;
        public Transform HealSource;
        public float CurrentHealth;
        public float PreviousHealth;
    }
}