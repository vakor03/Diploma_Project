using _Project.Features.ExperienceModule;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.StatsModule;
using _Project.Features.UIModule.Windows;
using _Project.Infrastructure.MVP.Core;
using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GameplayStates;

namespace _Project.Infrastructure.StateMachines.Scripts.GameplayStates {
    public class ExploreLevelState : IState {
        private readonly IWindowService _windowService;
        private readonly ExperienceModel _experienceModel;
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly PlayerStatsModel _playerStatsModel;
        
        public ExploreLevelState(IWindowService windowService, ExperienceModel experienceModel, GameplayStateMachine gameplayStateMachine, PlayerStatsModel playerStatsModel) {
            _windowService = windowService;
            _experienceModel = experienceModel;
            _gameplayStateMachine = gameplayStateMachine;
            _playerStatsModel = playerStatsModel;
        }

        public void Enter() {
            _experienceModel.OnLevelUp += ShowLevelUpWindow;
            _playerStatsModel.Stats.OnStatChanged += HandleSomeStatChanged;
            _windowService.ShowWindow<HUDWindow>();

            if (_playerStatsModel.Stats[EntityStats.CurrentHealth] <= 0)
                _gameplayStateMachine.Enter<GameOverState>();
        }

        private void ShowLevelUpWindow() =>
            _gameplayStateMachine.Enter<ChooseUpgradeState>();

        public void Exit() {
            _windowService.HideWindow<HUDWindow>();
            _playerStatsModel.Stats.OnStatChanged -= HandleSomeStatChanged;
            _experienceModel.OnLevelUp -= ShowLevelUpWindow;
        }

        private void HandleSomeStatChanged(EntityStats stat, float value) {
            if (stat == EntityStats.CurrentHealth && value <= 0)
                _gameplayStateMachine.Enter<GameOverState>();
        }
    }
}