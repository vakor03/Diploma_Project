using _Project.Infrastructure.MVP.Core;
using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GlobalStates;
using JetBrains.Annotations;

namespace _Project.Features.UIModule.MainMenuUI {
    [PublicAPI]
    public class MainMenuPresenter : PresenterBehaviour<MainMenuViewBase> {
        private readonly GlobalStateMachine _globalStateMachine;
    
        public MainMenuPresenter(GlobalStateMachine globalStateMachine) =>
            _globalStateMachine = globalStateMachine;

        public override void OnViewSet() {
            View.OnPlayButtonClicked += OnPlayButtonClicked;
            View.OnExitButtonClicked += OnExitButtonClicked;
        }

        public override void OnDisposed() {
            View.OnPlayButtonClicked -= OnPlayButtonClicked;
            View.OnExitButtonClicked -= OnExitButtonClicked;
        }

        private void OnPlayButtonClicked() =>
            _globalStateMachine.Enter<GameplayState>();

        private void OnExitButtonClicked() =>
            _globalStateMachine.Enter<QuitGameState>();
    }
}