using _Project.Features.CameraModule;
using _Project.Features.Enemy;
using _Project.Features.PlayerModule;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.UIModule.Windows;
using _Project.Features.VisualsModule.Scripts;
using _Project.Infrastructure.MVP.Core;
using _Project.Scripts.Infrastructure.StateMachines;
using Global.Helpers.Scripts;
using UnityEngine;
using Zenject;

namespace _Project.Infrastructure.StateMachines.Scripts.GameplayStates {
    public class SpawnEntitiesState : IState {
        private readonly IPlayerSpawnerService _playerSpawnerService;
        private readonly IPlayerSpawnPointsService _playerSpawnPointsService;
        private readonly ICameraSpawnService _cameraSpawnService;
        private readonly ICameraService _cameraService;
        private readonly IWindowService _windowService;
        private readonly EnemyObjectPool _enemyObjectPool;
        private readonly EnemySpawnPointsModel _enemySpawnPointsModel;
        private readonly IInstantiator _instantiator;
        private readonly VisualsConfiguration _visualsConfiguration;

        public SpawnEntitiesState(IPlayerSpawnerService playerSpawnerService,
                                  IPlayerSpawnPointsService playerSpawnPointsService, ICameraSpawnService cameraSpawnService,
                                  ICameraService cameraService, IWindowService windowService, EnemyObjectPool enemyObjectPool,
                                  EnemySpawnPointsModel enemySpawnPointsModel, IInstantiator instantiator,
                                  VisualsConfiguration visualsConfiguration) {
            _playerSpawnerService = playerSpawnerService;
            _playerSpawnPointsService = playerSpawnPointsService;
            _cameraSpawnService = cameraSpawnService;
            _cameraService = cameraService;
            _windowService = windowService;
            _enemyObjectPool = enemyObjectPool;
            _enemySpawnPointsModel = enemySpawnPointsModel;
            _instantiator = instantiator;
            _visualsConfiguration = visualsConfiguration;
        }

        public void Enter() {
            _cameraSpawnService.SpawnCamera();
            Vector3 playerSpawnPoint = _playerSpawnPointsService.GetPlayerSpawnPoint();
            Player player = _playerSpawnerService.SpawnPlayerAt(playerSpawnPoint);
            foreach (Vector3 spawnPoint in _enemySpawnPointsModel.SpawnPoints)
                _enemyObjectPool.Get(EnemyType.ShadowOfStorms)
                    .With(el => el.transform.position = spawnPoint);

            _cameraService.FollowTarget(player.transform);
            _instantiator.InstantiatePrefab(_visualsConfiguration.BackgroundPrefab)
                .transform.position = player.transform.position;

            _windowService.ShowWindow<HUDWindow>();
        }
    }
}