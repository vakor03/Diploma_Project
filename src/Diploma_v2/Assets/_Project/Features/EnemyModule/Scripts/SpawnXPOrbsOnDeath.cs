using System;
using _Project.Features.EnemyModule;
using _Project.Features.ExperienceModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Features.Enemy {
    public class SpawnXPOrbsOnDeath : MonoBehaviour {
        private IXPOrbSpawnService _xpOrbSpawnService;
        private EnemyXPConfiguration _xpConfiguration;

        private IDamageable _damageable;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private MonoPooledEnemy _monoPooledEnemy;
        
        [Inject]
        private void InjectDependencies(IXPOrbSpawnService xpOrbSpawnService, EnemyXPConfiguration enemyXPConfiguration) {
            _xpOrbSpawnService = xpOrbSpawnService;
            _xpConfiguration = enemyXPConfiguration;
        }

        private void Awake() =>
            _damageable = GetComponent<IDamageable>();

        private void OnEnable() =>
            _damageable.OnDeath += HandleOnDeath;

        private void OnDisable() =>
            _damageable.OnDeath -= HandleOnDeath;

        [Button]
        private void HandleOnDeath() =>
            _xpOrbSpawnService.SpawnXPOrb(_xpConfiguration.XPOnDeath[_monoPooledEnemy.Type],_spawnPoint.position);
    }
}