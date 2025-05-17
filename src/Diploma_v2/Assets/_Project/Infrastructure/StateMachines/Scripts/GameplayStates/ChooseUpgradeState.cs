using System.Collections.Generic;
using _Project.Features.ExperienceModule;
using _Project.Features.GameTimeModule;
using _Project.Features.UIModule.ChooseUpgradesUI;
using _Project.Features.UIModule.SingleUpgradeUI;
using _Project.Features.UIModule.Windows;
using _Project.Features.UpgradesModule.API;
using _Project.Features.UpgradesModule.UpgradePools;
using _Project.Infrastructure.MVP.Core;
using _Project.Scripts.Infrastructure.StateMachines;

namespace _Project.Infrastructure.StateMachines.Scripts.GameplayStates {
    public class ChooseUpgradeState : IState {
        private readonly IWindowService _windowService;
        private readonly IGamePauseService _gamePauseService;
        private readonly IUpgradePoolService _upgradePool;
        private readonly UpgradesToShowModel _upgradesToShowModel;
        private readonly ExperienceModel _experienceModel;
        private readonly UpgradeEvents _upgradeEvents;
        private readonly GameplayStateMachine _gameplayStateMachine;
        
        public ChooseUpgradeState(IWindowService windowService, IGamePauseService gamePauseService, UpgradesToShowModel upgradesToShowModel, IUpgradePoolService upgradePool, ExperienceModel experienceModel, GameplayStateMachine gameplayStateMachine, UpgradeEvents upgradeEvents) {
            _windowService = windowService;
            _gamePauseService = gamePauseService;
            _upgradesToShowModel = upgradesToShowModel;
            _upgradePool = upgradePool;
            _experienceModel = experienceModel;
            _gameplayStateMachine = gameplayStateMachine;
            _upgradeEvents = upgradeEvents;
        }

        public void Enter() {
            _upgradeEvents.OnUpgradeClaimed += ProceedToExploreLevel;
            List<UpgradeData> upgrades = _upgradePool.GetRandomUpgrades(_experienceModel.CurrentLevel, 0, 3);
            _upgradesToShowModel.SetUpgradesToShow(upgrades.ConvertAll(el=>el.upgradeId));
            _gamePauseService.StopTime();
            _windowService.ShowWindow<ChooseUpgradeWindow>();
        }

        private void ProceedToExploreLevel() =>
            _gameplayStateMachine.Enter<ExploreLevelState>();

        public void Exit() {
            _windowService.HideWindow<ChooseUpgradeWindow>();
            _gamePauseService.ResumeTime();
        }
    }
}