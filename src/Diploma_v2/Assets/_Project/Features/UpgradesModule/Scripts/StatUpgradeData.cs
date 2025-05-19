using System;
using _Project.Features.StatsModule;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class StatUpgradeData
    {
        public EntityStats statType;
        public float baseValue = 10f;
        public float valuePerLevel = 5f;
        public bool isPercentage = false;
    }
}