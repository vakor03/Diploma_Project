using _Project.Infrastructure.MVP.Core;
using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GlobalStates;
using JetBrains.Annotations;

namespace _Project.Features.UIModule.GameOverUI {
    [PublicAPI]
    public class GameOverPresenter : PresenterBehaviour<GameOverViewBase> {
        private readonly GlobalStateMachine _globalStateMachine;
        
        public GameOverPresenter(GlobalStateMachine globalStateMachine) =>
            _globalStateMachine = globalStateMachine;

        public override void OnViewSet() {
            View.OnMainMenuButtonClicked += HandleOnMainMenuButtonClicked;
            View.OnRestartButtonClicked += HandleOnRestartButtonClicked;
        }

        public override void OnDisposed() {
            View.OnMainMenuButtonClicked -= HandleOnMainMenuButtonClicked;
            View.OnRestartButtonClicked -= HandleOnRestartButtonClicked;
        }
        
        private void HandleOnMainMenuButtonClicked() =>
            _globalStateMachine.Enter<MainMenuState>();

        private void HandleOnRestartButtonClicked() =>
            _globalStateMachine.Enter<GameplayState>();
    }
}