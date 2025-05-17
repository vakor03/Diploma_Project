using System;
using _Project.Features.EnemyModule.EnemyPool;
using UnityEngine;
using Zenject;

namespace _Project.Features.DamageModule {
    public class ReleaseEnemyAfterDeathAnimationEnd : MonoBehaviour {
        private EnemyObjectPool _enemyPool;
        [SerializeField] private MonoPooledEnemy _monoPooledEnemy;
        [SerializeField] private EnemyAnimationEvents _enemyAnimationEvents;

        [Inject]
        private void InjectDependencies(EnemyObjectPool enemyPool) =>
            _enemyPool = enemyPool;

        private void OnEnable() =>
            _enemyAnimationEvents.DeathAnimationEnd += ReleaseEnemy;

        private void OnDisable() =>
            _enemyAnimationEvents.DeathAnimationEnd -= ReleaseEnemy;

        private void ReleaseEnemy() =>
            _enemyPool.Release(_monoPooledEnemy);
    }
}