using System;
using System.Collections.Generic;

namespace _Project.Features.StatsModule {
    public interface IStatService<T> where T : Enum
    {
        public float GetStat(T statType);
        public void SetStat(T statType, float value);
        public void ModifyStat(T statType, float value);
        public void ModifyStatPercentage(T statType, float percentage);
        public void ResetStat(T statType, float baseValue);
        public void ResetAllStats(Dictionary<T, float> baseValues);
        public Dictionary<T, float> GetAllStats();
    
        public event Action<T, float> OnStatChanged;
    }
}