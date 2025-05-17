using System;
using UnityEngine;

namespace _Project.Features.ExperienceModule.Orbs {
    [CreateAssetMenu(fileName = nameof(XPOrbConfiguration) + "_Default",
        menuName = "Configurations/ExperienceModule/" + nameof(XPOrbConfiguration))]
    public class XPOrbConfiguration : ScriptableObject
    {
        [SerializeField] private XPOrbData[] orbData;
    
        [Header("Spawn Settings")]
        [SerializeField] private float spawnRadius = 0.5f;
    
        public float SpawnRadius => spawnRadius;
        
        public XPOrbData GetOrbDataByType(XPOrbType type)
        {
            foreach (XPOrbData data in orbData)
                if (data.orbType == type)
                    return data;
            throw new ArgumentException("No XPOrbData found for type: " + type);
        }
    }
}