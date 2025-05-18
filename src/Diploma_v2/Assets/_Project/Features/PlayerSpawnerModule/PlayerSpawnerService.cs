using _Project.Features.PlayerModule;
using _Project.Features.StatsModule;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using Zenject;

namespace _Project.Features.PlayerSpawnerModule
{
    public class PlayerSpawnerService : IPlayerSpawnerService
    {
        private readonly IStaticDataService  _staticDataService;
        private readonly PlayerTransformDataHolder _playerTransformDataHolder;
        private readonly DiContainer _parentContainer;
        private readonly PlayerStatsModel _playerStatsModel;
        
        public PlayerSpawnerService(IStaticDataService staticDataService, PlayerTransformDataHolder playerTransformDataHolder, DiContainer parentContainer, PlayerStatsModel playerStatsModel) {
            _staticDataService = staticDataService;
            _playerTransformDataHolder = playerTransformDataHolder;
            _parentContainer = parentContainer;
            _playerStatsModel = playerStatsModel;
        }

        public Player SpawnPlayerAt(Vector3 position)
        {
            Player playerPrefab = _staticDataService.GetPlayerPrefab();
            
            StatDataHolder<EntityStats> statsHolder = _playerStatsModel.Stats;
            
            DiContainer subContainer = _parentContainer;
            subContainer.BindInstance(statsHolder).AsSingle();

            Player instantiated = subContainer.InstantiatePrefabForComponent<Player>(playerPrefab);
            instantiated.transform.position = position;
            
            _playerTransformDataHolder.Player = instantiated.transform;
            
            return instantiated;
        }
    }

    public class PlayerStatsModel : IModel {
        public StatDataHolder<EntityStats> Stats { get; set; } = new();
    }

    public interface IPlayerStatInitializeService {
        public void InitializePlayerStats();
    }

    public class PlayerStatInitializeService : IPlayerStatInitializeService {
        private readonly DefaultStatsDatabase _defaultStatsDatabase;
        private readonly PlayerStatsModel _playerStatsModel;
        public PlayerStatInitializeService(DefaultStatsDatabase defaultStatsDatabase, PlayerStatsModel playerStatsModel) {
            _defaultStatsDatabase = defaultStatsDatabase;
            _playerStatsModel = playerStatsModel;
        }

        public void InitializePlayerStats() {
            DefaultEntityStats playerStats = _defaultStatsDatabase.GetPlayerStats();
            foreach ((EntityStats stat, float value) in playerStats.stats)
                _playerStatsModel.Stats.SetStat(stat, value);
            
            _playerStatsModel.Stats.SetStat(EntityStats.CurrentHealth, _playerStatsModel.Stats[EntityStats.MaxHealth]);
        }
    }

    public class PlayerStatsInstaller : Installer<PlayerStatsInstaller> {
        public override void InstallBindings() {
            Container.Bind<IPlayerStatInitializeService>().To<PlayerStatInitializeService>().AsSingle();
        }
    }

    public class PlayerTransformDataHolder {
        public Transform Player { get; set; }
    }
}