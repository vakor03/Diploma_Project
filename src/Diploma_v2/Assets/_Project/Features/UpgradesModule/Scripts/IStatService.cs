using UnityEngine;

namespace _Project.Features.UpgradesModule {
    public interface IStatService
    {
        public void ModifyStat(GameObject target, StatType statType, float value, bool isPercentage);
        public float GetStat(GameObject target, StatType statType);
    }
}