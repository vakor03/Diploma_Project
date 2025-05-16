using _Project.Features.Enemy;
using UnityEngine;
using Zenject;

namespace _Project.Features.DamageModule {
    public class ReleaseEnemyAfterDeath : MonoBehaviour {
        private EnemyObjectPool _enemyPool;
        [SerializeField] private MonoPooledEnemy _monoPooledEnemy;
        [SerializeField] private SimpleMonoDamageable _simpleMonoDamageable;


        [Inject]
        private void InjectDependencies(EnemyObjectPool enemyPool) =>
            _enemyPool = enemyPool;

        private void OnEnable() =>
            _simpleMonoDamageable.OnDeath += ReleaseEnemy;

        private void OnDisable() =>
            _simpleMonoDamageable.OnDeath -= ReleaseEnemy;

        private void ReleaseEnemy() {
            _enemyPool.Release(_monoPooledEnemy);
        }
    }
}