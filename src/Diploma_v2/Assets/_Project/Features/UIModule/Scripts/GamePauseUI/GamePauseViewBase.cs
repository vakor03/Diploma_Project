using System;
using _Project.Infrastructure.MVP.Core;

namespace _Project.Features.UIModule.GamePauseUI {
    public abstract class GamePauseViewBase : ViewBehaviour {
        public event Action OnResumeButtonClicked;
        public event Action OnBackToMainMenuButtonClicked;
        public event Action OnExitButtonClicked;
    
        protected void InvokeOnResumeButtonClicked() =>
            OnResumeButtonClicked?.Invoke();
    
        protected void InvokeOnBackToMainMenuButtonClicked() =>
            OnBackToMainMenuButtonClicked?.Invoke();
    
        protected void InvokeOnExitButtonClicked() =>
            OnExitButtonClicked?.Invoke();
    }
}