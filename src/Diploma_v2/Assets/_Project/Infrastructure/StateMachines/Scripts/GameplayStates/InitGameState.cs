using _Project.Features.LevelGeneratorModule;
using _Project.Features.PlayerSpawnerModule;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.StateMachines.GameplayStates
{
    public class InitGameState : IState
    {
        private readonly IPlayerSpawnerService _playerSpawnerService;
        private readonly ILevelGenerationService _levelGenerationService;
        private readonly IPlayerSpawnPointsService _playerSpawnPointsService;

        public InitGameState(IPlayerSpawnPointsService playerSpawnPointsService,
            ILevelGenerationService levelGenerationService, IPlayerSpawnerService playerSpawnerService)
        {
            _playerSpawnPointsService = playerSpawnPointsService;
            _levelGenerationService = levelGenerationService;
            _playerSpawnerService = playerSpawnerService;
        }

        public void Enter()
        {
            Vector3 playerSpawnPoint = _playerSpawnPointsService.GetPlayerSpawnPoint();
            _playerSpawnerService.SpawnPlayerAt(playerSpawnPoint);
            _levelGenerationService.Generate();
        }

        public void Exit()
        {
        }
    }
}