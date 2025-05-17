using _Project.Extensions.ZenjectExtensions;
using _Project.Features.ObjectPoolModule;
using _Project.Features.StatsModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using Zenject;

namespace _Project.Features.Enemy {
    public class EnemyPoolInstaller : Installer<EnemyPoolInstaller> {

        public override void InstallBindings() {
            Container.BindInterfacesAndSelfToFromAddressables<EnemyPoolConfiguration>(AssetPath.Configuration.ENEMY_POOL_CONFIGURATION)
                .AsSingle();
            Container
                .Bind<IGenericPooledObjectFactory<MonoPooledEnemy, EnemyType>>()
                .To<EnemyPooledObjectFactory>()
                .AsSingle();


            // Container.BindInterfacesAndSelfTo<GenericPooledObjectFactory<MonoPooledEnemy, EnemyType>>()
            //     .AsSingle();

            Container.BindInterfacesAndSelfTo<GenericPoolFactory<MonoPooledEnemy, EnemyType>>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyObjectPool>()
                .AsSingle();
        }
    }


    public class EnemyPooledObjectFactory 
        : IGenericPooledObjectFactory<MonoPooledEnemy, EnemyType>
    {
        readonly DiContainer                          _parentContainer;
        readonly IObjectPoolConfiguration<EnemyType> _poolConfig;

        [Inject]
        public EnemyPooledObjectFactory(
            DiContainer parentContainer,
            IObjectPoolConfiguration<EnemyType> poolConfig)
        {
            _parentContainer = parentContainer;
            _poolConfig      = poolConfig;
        }

        public MonoPooledEnemy Create(EnemyType type, Transform parentTransform)
        {
            ObjectPoolSettings cfg         = _poolConfig.GetPoolConfiguration(type);
            GameObject prefab      = cfg.Prefab;
            StatDataHolder<EntityStats> statsHolder = new StatDataHolder<EntityStats>();
            statsHolder.SetStat(EntityStats.MaxHealth, 20f);
        
            DiContainer subContainer = _parentContainer.CreateSubContainer();

            subContainer.BindInstance(statsHolder).AsSingle();

            GameObject go = subContainer.InstantiatePrefab(prefab, parentTransform);

            MonoPooledEnemy monoPooledEnemy = go.GetComponent<MonoPooledEnemy>();
            monoPooledEnemy.Type = type;
            return monoPooledEnemy;
        }
    }


}