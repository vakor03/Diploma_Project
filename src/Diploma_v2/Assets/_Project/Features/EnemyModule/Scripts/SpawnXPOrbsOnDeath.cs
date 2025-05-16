using _Project.Features.ExperienceModule;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Features.Enemy {
    public class SpawnXPOrbsOnDeath : MonoBehaviour {
        private IXPOrbSpawnService _xpOrbSpawnService;

        [SerializeField] private float _experienceAmount;
        [SerializeField] private EntityHealthController _entityHealthController;
        [SerializeField] private Transform _spawnPoint;
        
        [Inject]
        private void InjectDependencies(IXPOrbSpawnService xpOrbSpawnService) =>
            _xpOrbSpawnService = xpOrbSpawnService;

        private void OnEnable() =>
            _entityHealthController.Death += HandleOnDeath;

        private void OnDisable() =>
            _entityHealthController.Death -= HandleOnDeath;

        [Button]
        private void HandleOnDeath(DeathArgs obj) =>
            _xpOrbSpawnService.SpawnXPOrb(_experienceAmount, _spawnPoint.position);
    }
}