using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using Zenject;

namespace _Project.Features.Enemy.EnemySpawner {
    public interface IEnemySpawnerService {
        public Enemy SpawnEnemyAt(Vector3 spawnPoint, Transform parent);
    }
 
    public class EnemySpawnerService : IEnemySpawnerService {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService  _staticDataService;

        public EnemySpawnerService(IInstantiator instantiator, IStaticDataService staticDataService)
        {
            _instantiator = instantiator;
            _staticDataService = staticDataService;
        }

        public Enemy SpawnEnemyAt(Vector3 position, Transform parent)
        {
            Enemy enemyPrefab = _staticDataService.GetEnemyPrefab();

            Enemy instantiated = _instantiator.InstantiatePrefabForComponent<Enemy>(enemyPrefab, parent);
            instantiated.transform.position = position;
            
            return instantiated;
        }
    }

    public class EnemySpawnerInstaller : Installer<EnemySpawnerInstaller> {
        public override void InstallBindings() {
            Container.Bind<IEnemySpawnerService>().To<EnemySpawnerService>().AsSingle();
        }
    }
}