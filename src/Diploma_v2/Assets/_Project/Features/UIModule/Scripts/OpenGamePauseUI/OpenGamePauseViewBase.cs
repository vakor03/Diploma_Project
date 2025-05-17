using System;
using _Project.Infrastructure.MVP.Core;

namespace _Project.Features.UIModule.OpenGamePauseUI {
    public abstract class OpenGamePauseViewBase : ViewBehaviour {
        public event Action OnGamePauseButtonClicked;

        protected void InvokeOnGamePauseButtonClicked() =>
            OnGamePauseButtonClicked?.Invoke();
    }
}