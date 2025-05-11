using System;
using _Project.Infrastructure.MVP.Core;

namespace _Project.Features.UIModule.GamePauseUI {
    public abstract class OpenGamePauseViewBase : ViewBehaviour {
        public event Action OnGamePauseButtonClicked;

        protected void InvokeOnGamePauseButtonClicked() =>
            OnGamePauseButtonClicked?.Invoke();
    }
}