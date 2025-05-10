using _Project.Features.CameraModule;
using _Project.Features.Enemy.EnemySpawner;
using _Project.Features.LevelGeneratorModule;
using _Project.Features.PlayerModule;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.UIModule.Scripts;
using _Project.Infrastructure.MVP.Core;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.StateMachines.GameplayStates
{
    public class InitGameState : IState
    {
        private readonly IPlayerSpawnerService _playerSpawnerService;
        private readonly ILevelGenerationService _levelGenerationService;
        private readonly IPlayerSpawnPointsService _playerSpawnPointsService;
        private readonly ICameraSpawnService _cameraSpawnService;
        private readonly ICameraService _cameraService;
        private readonly IWindowService _windowService;
        private readonly IEnemySpawnerService _enemySpawnerService;
        private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

        public InitGameState(IPlayerSpawnPointsService playerSpawnPointsService,
            ILevelGenerationService levelGenerationService, IPlayerSpawnerService playerSpawnerService, ICameraSpawnService cameraSpawnService, ICameraService cameraService, IWindowService windowService, EnemySpawnPointsModel enemySpawnPointsModel, IEnemySpawnerService enemySpawnerService)
        {
            _playerSpawnPointsService = playerSpawnPointsService;
            _levelGenerationService = levelGenerationService;
            _playerSpawnerService = playerSpawnerService;
            _cameraSpawnService = cameraSpawnService;
            _cameraService = cameraService;
            _windowService = windowService;
            _enemySpawnPointsModel = enemySpawnPointsModel;
            _enemySpawnerService = enemySpawnerService;
        }

        public void Enter()
        {
            _cameraSpawnService.SpawnCamera();
            _levelGenerationService.Generate();
            Vector3 playerSpawnPoint = _playerSpawnPointsService.GetPlayerSpawnPoint();
            Player player = _playerSpawnerService.SpawnPlayerAt(playerSpawnPoint);
            foreach (Vector3 spawnPoint in _enemySpawnPointsModel.SpawnPoints)
                _enemySpawnerService.SpawnEnemyAt(spawnPoint);
            _cameraService.FollowTarget(player.transform);
            _windowService.ShowWindow<HUDWindow>();
        }

        public void Exit()
        {
        }
    }
}