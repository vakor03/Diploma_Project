using UnityEngine;

namespace _Project.Features.Enemy {
    [CreateAssetMenu(fileName = "EntityHealthData", menuName = "Health/Entity Health Data")]
    public class EntityHealthData : ScriptableObject
    {
        [Header("Health Settings")]
        public float maxHealth = 100f;
        public float startingHealth = 0f; // 0 means start with max health
    
        [Header("Invulnerability")]
        public float invulnerabilityTime = 0.5f;
    
        [Header("Death Settings")]
        public bool destroyOnDeath = true;
        public float deathDelay = 0f;
    
        [Header("Resistances")]
        public DamageResistances damageResistances;
    }
}