using _Project.Scripts.Core.Gameplay;

namespace _Project.Scripts.Infrastructure.StateMachines.GameplayStates {
    public class StartGameState : IState {
        private readonly PlayerExperienceModel _playerExperienceModel;
        private readonly IPowerUpSelectionService _powerUpSelectionService;

        public StartGameState(PlayerExperienceModel playerExperienceModel, IPowerUpSelectionService powerUpSelectionService) {
            _playerExperienceModel = playerExperienceModel;
            _powerUpSelectionService = powerUpSelectionService;
        }

        public void Enter() =>
            _playerExperienceModel.OnLevelChanged += _powerUpSelectionService.StartPowerUpSelection;

        public void Exit() =>
            _playerExperienceModel.OnLevelChanged -= _powerUpSelectionService.StartPowerUpSelection;
    }
}