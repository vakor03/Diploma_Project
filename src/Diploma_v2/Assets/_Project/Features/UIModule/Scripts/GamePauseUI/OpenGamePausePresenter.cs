using _Project.Infrastructure.MVP.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Features.UIModule.GamePauseUI {
    [PublicAPI]
    public class OpenGamePausePresenter : PresenterBehaviour<OpenGamePauseViewBase> {
        public override void OnViewSet() =>
            View.OnGamePauseButtonClicked += OpenGamePauseMenu;

        public override void OnDisposed() =>
            View.OnGamePauseButtonClicked -= OpenGamePauseMenu;

        private void OpenGamePauseMenu() {
            Debug.LogError($"[OpenGamePausePresenter.OpenGamePauseMenu Line 16]");
        }
    }
}