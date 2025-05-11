using System;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class StatUpgradeData
    {
        public StatType statType;
        public float baseValue = 10f;
        public float valuePerLevel = 5f;
        public bool isPercentage = false;
    }
}