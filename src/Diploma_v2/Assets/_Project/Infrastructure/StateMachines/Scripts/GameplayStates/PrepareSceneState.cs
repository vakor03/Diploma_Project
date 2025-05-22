using _Project.Features.CameraModule;
using _Project.Features.EnemyModule;
using _Project.Features.EnemyModule.EnemyPool;
using _Project.Features.GameTimeModule;
using _Project.Features.PlayerModule;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.VisualsModule;
using _Project.Scripts.Infrastructure.StateMachines;
using Global.Helpers.Scripts;
using UnityEngine;
using Zenject;

namespace _Project.Infrastructure.StateMachines.Scripts.GameplayStates {
    public class PrepareSceneState : IState {
        private readonly IPlayerSpawnerService _playerSpawnerService;
        private readonly IPlayerSpawnPointsService _playerSpawnPointsService;
        private readonly ICameraSpawnService _cameraSpawnService;
        private readonly ICameraService _cameraService;
        private readonly EnemyObjectPool _enemyObjectPool;
        private readonly EnemySpawnPointsModel _enemySpawnPointsModel;
        private readonly IInstantiator _instantiator;
        private readonly VisualsConfiguration _visualsConfiguration;
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly IPlayerStatInitializeService _playerStatInitializeService;
        private readonly IGamePauseService _gamePauseService;

        public PrepareSceneState(IPlayerSpawnerService playerSpawnerService,
                                  IPlayerSpawnPointsService playerSpawnPointsService, ICameraSpawnService cameraSpawnService,
                                  ICameraService cameraService, EnemyObjectPool enemyObjectPool,
                                  EnemySpawnPointsModel enemySpawnPointsModel, IInstantiator instantiator,
                                  VisualsConfiguration visualsConfiguration, GameplayStateMachine gameplayStateMachine, IPlayerStatInitializeService playerStatInitializeService, IGamePauseService gamePauseService) {
            _playerSpawnerService = playerSpawnerService;
            _playerSpawnPointsService = playerSpawnPointsService;
            _cameraSpawnService = cameraSpawnService;
            _cameraService = cameraService;
            _enemyObjectPool = enemyObjectPool;
            _enemySpawnPointsModel = enemySpawnPointsModel;
            _instantiator = instantiator;
            _visualsConfiguration = visualsConfiguration;
            _gameplayStateMachine = gameplayStateMachine;
            _playerStatInitializeService = playerStatInitializeService;
            _gamePauseService = gamePauseService;
        }

        public void Enter() {
            _playerStatInitializeService.InitializePlayerStats();
            _cameraSpawnService.SpawnCamera();
            Vector3 playerSpawnPoint = _playerSpawnPointsService.GetPlayerSpawnPoint();
            Player player = _playerSpawnerService.SpawnPlayerAt(playerSpawnPoint);
            for (int index = 0; index < _enemySpawnPointsModel.SpawnPoints.Count; index++) {
                Vector3 spawnPoint = _enemySpawnPointsModel.SpawnPoints[index];
                GameObject testGo = new GameObject("Enemy" + index);
                testGo.transform.position = spawnPoint;
                _enemyObjectPool.Get(EnemyType.OnePlaceGuardRobot)
                    .With(el => el.transform.position = spawnPoint);
            }

            _cameraService.FollowTarget(player.transform);
            _instantiator.InstantiatePrefab(_visualsConfiguration.BackgroundPrefab)
                .transform.position = player.transform.position;
            
            _gamePauseService.ForceResumeTime();
            
            _gameplayStateMachine.Enter<ExploreLevelState>();
        }
    }
}