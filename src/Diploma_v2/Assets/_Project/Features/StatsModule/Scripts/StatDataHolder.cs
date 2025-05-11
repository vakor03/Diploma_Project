using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure;

namespace _Project.Features.StatsModule {
    [Serializable]
    public class StatDataHolder<T> : IModel where T : Enum
    {
        private Dictionary<T, float> _stats = new Dictionary<T, float>();
        private Dictionary<T, float> _baseStats = new Dictionary<T, float>();
    
        public StatDataHolder() =>
            InitializeStats();

        private void InitializeStats()
        {
            foreach (T stat in Enum.GetValues(typeof(T)))
            {
                _stats[stat] = 0f;
                _baseStats[stat] = 0f;
            }
        }
    
        public float GetStat(T statType)
        {
            _stats.TryGetValue(statType, out float value);
            return value;
        }
    
        public void SetStat(T statType, float value)
        {
            _stats[statType] = value;
            _baseStats[statType] = value; // Update base value as well
        }
    
        public void ModifyStat(T statType, float value)
        {
            _stats[statType] = _stats.GetValueOrDefault(statType, 0f) + value;
        }
    
        public void ModifyStatPercentage(T statType, float percentage)
        {
            float baseValue = _baseStats.GetValueOrDefault(statType, 0f);
            float currentValue = _stats.GetValueOrDefault(statType, 0f);
            float modification = baseValue * (percentage / 100f);
            _stats[statType] = currentValue + modification;
        }
    
        public void ResetStat(T statType, float baseValue)
        {
            _stats[statType] = baseValue;
            _baseStats[statType] = baseValue;
        }
    
        public void ResetAllStats(Dictionary<T, float> baseValues)
        {
            foreach (var kvp in baseValues)
            {
                _stats[kvp.Key] = kvp.Value;
                _baseStats[kvp.Key] = kvp.Value;
            }
        }
    
        public Dictionary<T, float> GetAllStats()
        {
            return new Dictionary<T, float>(_stats);
        }
    
        public Dictionary<T, float> GetBaseStats()
        {
            return new Dictionary<T, float>(_baseStats);
        }
    }
}