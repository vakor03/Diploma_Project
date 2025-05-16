using System;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public abstract class MonoDamageable : MonoBehaviour,IDamageable {
        public abstract event Action<float> OnTakeDamage;
        public abstract event Action OnDeath;
        public abstract event Action OnAfterDeath;
        public abstract void TakeDamage(float damage);
    }
}