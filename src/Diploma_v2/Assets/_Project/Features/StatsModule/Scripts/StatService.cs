using System;
using System.Collections.Generic;

namespace _Project.Features.StatsModule {
    public class StatService<T> : IStatService<T> where T : Enum
    {
        private readonly StatDataHolder<T> _dataHolder;
    
        public event Action<T, float> OnStatChanged;
    
        public StatService(StatDataHolder<T> dataHolder) =>
            _dataHolder = dataHolder;

        public float GetStat(T statType) =>
            _dataHolder.GetStat(statType);

        public void SetStat(T statType, float value)
        {
            _dataHolder.SetStat(statType, value);
            OnStatChanged?.Invoke(statType, value);
        }
    
        public void ModifyStat(T statType, float value)
        {
            _dataHolder.ModifyStat(statType, value);
            OnStatChanged?.Invoke(statType, _dataHolder.GetStat(statType));
        }
    
        public void ModifyStatPercentage(T statType, float percentage)
        {
            _dataHolder.ModifyStatPercentage(statType, percentage);
            OnStatChanged?.Invoke(statType, _dataHolder.GetStat(statType));
        }
    
        public void ResetStat(T statType, float baseValue)
        {
            _dataHolder.ResetStat(statType, baseValue);
            OnStatChanged?.Invoke(statType, baseValue);
        }
    
        public void ResetAllStats(Dictionary<T, float> baseValues)
        {
            _dataHolder.ResetAllStats(baseValues);
        
            foreach (KeyValuePair<T, float> kvp in baseValues)
                OnStatChanged?.Invoke(kvp.Key, kvp.Value);
        }
    
        public Dictionary<T, float> GetAllStats() =>
            _dataHolder.GetAllStats();
    }
}