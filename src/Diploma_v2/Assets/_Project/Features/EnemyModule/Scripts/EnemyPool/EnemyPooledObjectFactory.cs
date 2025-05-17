using System.Collections.Generic;
using _Project.Features.ObjectPoolModule;
using _Project.Features.StatsModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.Enemy {
    public class EnemyPooledObjectFactory : IGenericPooledObjectFactory<MonoPooledEnemy, EnemyType> {
        private readonly DiContainer _parentContainer;
        private readonly IObjectPoolConfiguration<EnemyType> _poolConfig;
        private readonly DefaultStatsDatabase _statsDatabase;

        public EnemyPooledObjectFactory(
            DiContainer parentContainer,
            IObjectPoolConfiguration<EnemyType> poolConfig, DefaultStatsDatabase statsDatabase) {
            _parentContainer = parentContainer;
            _poolConfig = poolConfig;
            _statsDatabase = statsDatabase;
        }

        public MonoPooledEnemy Create(EnemyType type, Transform parentTransform) {
            ObjectPoolSettings cfg = _poolConfig.GetPoolConfiguration(type);
            GameObject prefab = cfg.Prefab;
            StatDataHolder<EntityStats> statsHolder = CreateStatDataHolder(type);

            DiContainer subContainer = _parentContainer.CreateSubContainer();

            subContainer.BindInstance(statsHolder).AsSingle();

            GameObject go = subContainer.InstantiatePrefab(prefab, parentTransform);

            MonoPooledEnemy monoPooledEnemy = go.GetComponent<MonoPooledEnemy>();
            monoPooledEnemy.Type = type;
            return monoPooledEnemy;
        }

        private StatDataHolder<EntityStats> CreateStatDataHolder(EnemyType type) {
            StatDataHolder<EntityStats> statsHolder = new StatDataHolder<EntityStats>();
            DefaultEnemyStats defaultEnemyStats = _statsDatabase.GetEnemyStats(type);
            if (defaultEnemyStats == null) {
                Debug.LogError($"No stats found for enemy type: {type}");
                return statsHolder;
            }

            foreach ((EntityStats stats, float value) in defaultEnemyStats.stats)
                statsHolder.SetStat(stats, value);

            return statsHolder;
        }
    }
}