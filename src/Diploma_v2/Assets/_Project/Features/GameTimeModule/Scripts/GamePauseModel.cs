using System;
using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Features.GameTimeModule {
    public class GamePauseModel : IModel
    {
        public bool IsPaused { get; private set; } = false;
        public int PauseCounter { get; private set; } = 0;
        public float OriginalTimeScale { get; private set; } = 1f;
    
        public event Action<bool> OnPauseStateChanged;

        public void IncrementPause()
        {
            PauseCounter++;
            UpdatePauseState();
        }
    
        public void DecrementPause()
        {
            if (PauseCounter > 0)
            {
                PauseCounter--;
                UpdatePauseState();
            }
        }
    
        public void ForceResume()
        {
            PauseCounter = 0;
            UpdatePauseState();
        }
    
        private void UpdatePauseState()
        {
            bool shouldBePaused = PauseCounter > 0;
        
            if (shouldBePaused != IsPaused)
            {
                IsPaused = shouldBePaused;
            
                if (IsPaused)
                {
                    OriginalTimeScale = Time.timeScale;
                    Time.timeScale = 0f;
                }
                else
                    Time.timeScale = OriginalTimeScale;
            
                OnPauseStateChanged?.Invoke(IsPaused);
            }
        }
    }
}