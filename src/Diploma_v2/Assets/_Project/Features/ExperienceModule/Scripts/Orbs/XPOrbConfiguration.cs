using System;
using UnityEngine;

namespace _Project.Features.ExperienceModule {
    [CreateAssetMenu(fileName = "XPOrbConfig", menuName = "Game/XP Orb Configuration")]
    public class XPOrbConfiguration : ScriptableObject
    {
        [SerializeField] private XPOrbData[] orbData;
    
        [Header("Spawn Settings")]
        [SerializeField] private float spawnRadius = 0.5f;
        [SerializeField] private int minOrbs = 1;
        [SerializeField] private int maxOrbs = 8;
    
        public float SpawnRadius => spawnRadius;
        public int MinOrbs => minOrbs;
        public int MaxOrbs => maxOrbs;
    
        public XPOrbData GetOrbDataForXP(float xpAmount)
        {
            for (int i = orbData.Length - 1; i >= 0; i--)
            {
                if (xpAmount >= orbData[i].minXP)
                    return orbData[i];
            }
        
            return orbData[0];
        }
    
        public XPOrbData GetOrbDataByType(XPOrbType type)
        {
            foreach (XPOrbData data in orbData)
                if (data.orbType == type)
                    return data;
            throw new ArgumentException("No XPOrbData found for type: " + type);
        }
    
        public int CalculateOrbCount(float totalXP)
        {
            int count = minOrbs;
        
            if (totalXP <= 5) count = 1;
            else if (totalXP <= 20) count = UnityEngine.Random.Range(2, 4);
            else if (totalXP <= 50) count = UnityEngine.Random.Range(3, 6);
            else count = UnityEngine.Random.Range(5, maxOrbs + 1);
        
            return Mathf.Clamp(count, minOrbs, maxOrbs);
        }
    }
}