using System;
using _Project.Infrastructure.MVP.Core;

namespace _Project.Features.UIModule.GameOverUI {
    public abstract class GameOverViewBase : ViewBehaviour {
        public event Action OnMainMenuButtonClicked;
        public event Action OnRestartButtonClicked;

        protected void InvokeOnMainMenuButtonClicked() =>
            OnMainMenuButtonClicked?.Invoke();

        protected void InvokeOnRestartButtonClicked() =>
            OnRestartButtonClicked?.Invoke();
    }
}