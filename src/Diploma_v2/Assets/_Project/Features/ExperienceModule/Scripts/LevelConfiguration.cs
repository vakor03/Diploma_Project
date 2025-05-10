using UnityEngine;

namespace _Project.Features.ExperienceModule {
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Configuration")]
    public class XPLevelConfiguration : ScriptableObject
    {
        [Header("Level Settings")]
        [SerializeField] private int firstLevelXP = 100;
        [SerializeField] private float xpMultiplier = 1.2f;
        [SerializeField] private int maxLevel = 100;
    
        public int FirstLevelXP => firstLevelXP;
        public float XPMultiplier => xpMultiplier;
        public int MaxLevel => maxLevel;
    
        public int GetXPForLevel(int level)
        {
            if (level <= 0) return 0;
            if (level > maxLevel) level = maxLevel;
        
            return Mathf.RoundToInt(firstLevelXP * Mathf.Pow(xpMultiplier, level - 1));
        }
    }
}