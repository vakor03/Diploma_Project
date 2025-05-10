using System;
using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Features.ExperienceModule {
    public class ExperienceModel : IModel
    {
        public int CurrentLevel { get; private set; } = 1;
        public int CurrentXP { get; private set; }
        public int MaxXP { get; private set; }

        public float XPPercentage => MaxXP > 0 ? (float)CurrentXP / MaxXP : 0f;
        
            
        public event Action<int> OnLevelChanged;
        public event Action OnLevelUp;
        public event Action<int> OnCurrentXPChanged;
    
        public void SetCurrentXP(int xp)
        {
            CurrentXP = xp;
            OnCurrentXPChanged?.Invoke(xp);
        }
        
        public void SetLevel(int level)
        {
            CurrentLevel = level;
            OnLevelChanged?.Invoke(level);
        }
        
        public void SetMaxXP(int xp) =>
            MaxXP = xp;

        public void SetLevelSilently(int level) =>
            CurrentLevel = level;

        public void IncrementLevel() {
            CurrentLevel++;
            OnLevelUp?.Invoke();
            OnLevelChanged?.Invoke(CurrentLevel);
        }
    }
}