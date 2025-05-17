using _Project.Features.ExperienceModule;
using _Project.Features.UIModule.Windows;
using _Project.Infrastructure.MVP.Core;
using _Project.Scripts.Infrastructure.StateMachines;

namespace _Project.Infrastructure.StateMachines.Scripts.GameplayStates {
    public class ExploreLevelState : IState {
        private readonly IWindowService _windowService;
        private readonly ExperienceModel _experienceModel;
        private readonly GameplayStateMachine _gameplayStateMachine;
        
        public ExploreLevelState(IWindowService windowService, ExperienceModel experienceModel, GameplayStateMachine gameplayStateMachine) {
            _windowService = windowService;
            _experienceModel = experienceModel;
            _gameplayStateMachine = gameplayStateMachine;
        }

        public void Enter() {
            _experienceModel.OnLevelUp += ShowLevelUpWindow;
            _windowService.ShowWindow<HUDWindow>();
        }

        private void ShowLevelUpWindow() =>
            _gameplayStateMachine.Enter<ChooseUpgradeState>();

        public void Exit() =>
            _windowService.HideWindow<HUDWindow>();
    }
}