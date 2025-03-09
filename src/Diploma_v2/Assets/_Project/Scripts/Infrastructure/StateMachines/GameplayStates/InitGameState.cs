using _Project.Scripts.Core.Gameplay;
using _Project.Scripts.Core.Windows;

namespace _Project.Scripts.Infrastructure.StateMachines.GameplayStates {
    public class InitGameState : IState {
        private readonly ICameraFollowService _cameraFollowService;
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly IPlayerExperienceService _playerExperienceService;
        private readonly IPlayerSpawnService _playerSpawnService;
        private readonly IWindowManager _windowManager;

        public InitGameState(IPlayerSpawnService playerSpawnService, GameplayStateMachine gameplayStateMachine, ICameraFollowService cameraFollowService, IWindowManager windowManager,
                             IPlayerExperienceService playerExperienceService) {
            _playerSpawnService = playerSpawnService;
            _gameplayStateMachine = gameplayStateMachine;
            _cameraFollowService = cameraFollowService;
            _windowManager = windowManager;
            _playerExperienceService = playerExperienceService;
        }

        public void Enter() {
            _playerSpawnService.SpawnPlayer();
            _cameraFollowService.FollowPlayer();
            _playerExperienceService.SetLevel(1);
            _windowManager.OpenWindow<GameHUDWindow>();
            _gameplayStateMachine.Enter<StartGameState>();
        }
    }
}