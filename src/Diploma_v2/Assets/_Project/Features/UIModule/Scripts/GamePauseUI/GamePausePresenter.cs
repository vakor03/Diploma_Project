using _Project.Infrastructure.MVP.Core;
using _Project.Infrastructure.StateMachines.Scripts.GameplayStates;
using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GlobalStates;
using JetBrains.Annotations;

namespace _Project.Features.UIModule.GamePauseUI {
    [PublicAPI]
    public class GamePausePresenter : PresenterBehaviour<GamePauseViewBase> {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly GlobalStateMachine _globalStateMachine;
    
        public GamePausePresenter(GlobalStateMachine globalStateMachine, GameplayStateMachine gameplayStateMachine) {
            _globalStateMachine = globalStateMachine;
            _gameplayStateMachine = gameplayStateMachine;
        }

        public override void OnViewSet() {
            View.OnResumeButtonClicked += OnResumeButtonClicked;
            View.OnBackToMainMenuButtonClicked += OnBackToMainMenuButtonClicked;
            View.OnExitButtonClicked += OnExitButtonClicked;
        }

        public override void OnDisposed() {
            View.OnResumeButtonClicked -= OnResumeButtonClicked;
            View.OnBackToMainMenuButtonClicked -= OnBackToMainMenuButtonClicked;
            View.OnExitButtonClicked -= OnExitButtonClicked;
        }

        private void OnResumeButtonClicked() =>
            _gameplayStateMachine.Enter<ExploreLevelState>();

        private void OnExitButtonClicked() =>
            _globalStateMachine.Enter<QuitGameState>();

        private void OnBackToMainMenuButtonClicked() =>
            _globalStateMachine.Enter<MainMenuState>();
    }
}