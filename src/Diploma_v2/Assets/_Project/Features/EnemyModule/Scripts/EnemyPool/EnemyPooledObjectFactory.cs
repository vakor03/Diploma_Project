using _Project.Features.ObjectPoolModule;
using _Project.Features.StatsModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.Enemy {
    public class EnemyPooledObjectFactory : IGenericPooledObjectFactory<MonoPooledEnemy, EnemyType> {
        readonly DiContainer _parentContainer;
        readonly IObjectPoolConfiguration<EnemyType> _poolConfig;

        public EnemyPooledObjectFactory(
            DiContainer parentContainer,
            IObjectPoolConfiguration<EnemyType> poolConfig) {
            _parentContainer = parentContainer;
            _poolConfig = poolConfig;
        }

        public MonoPooledEnemy Create(EnemyType type, Transform parentTransform) {
            ObjectPoolSettings cfg = _poolConfig.GetPoolConfiguration(type);
            GameObject prefab = cfg.Prefab;
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