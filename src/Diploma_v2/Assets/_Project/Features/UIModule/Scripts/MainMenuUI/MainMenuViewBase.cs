using System;
using _Project.Infrastructure.MVP.Core;

namespace _Project.Features.UIModule.MainMenuUI {
    public abstract class MainMenuViewBase : ViewBehaviour {
        public event Action OnPlayButtonClicked;
        public event Action OnExitButtonClicked;

        protected void InvokeOnPlayButtonClicked() =>
            OnPlayButtonClicked?.Invoke();
    
        protected void InvokeOnExitButtonClicked() =>
            OnExitButtonClicked?.Invoke();
    }
}