using _Project.Infrastructure.MVP.Core;
using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GameplayStates;
using JetBrains.Annotations;

namespace _Project.Features.UIModule.OpenGamePauseUI {
    [PublicAPI]
    public class OpenGamePausePresenter : PresenterBehaviour<OpenGamePauseViewBase> {
        private readonly GameplayStateMachine _gameplayStateMachine;
        
        public OpenGamePausePresenter(GameplayStateMachine gameplayStateMachine) =>
            _gameplayStateMachine = gameplayStateMachine;

        public override void OnViewSet() =>
            View.OnGamePauseButtonClicked += OpenGamePauseMenu;

        public override void OnDisposed() =>
            View.OnGamePauseButtonClicked -= OpenGamePauseMenu;

        private void OpenGamePauseMenu() =>
            _gameplayStateMachine.Enter<GamePauseState>();
    }
}