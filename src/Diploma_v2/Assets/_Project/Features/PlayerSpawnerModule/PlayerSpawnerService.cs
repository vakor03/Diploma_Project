using _Project.Features.PlayerModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using Zenject;

namespace _Project.Features.PlayerSpawnerModule
{
    public class PlayerSpawnerService : IPlayerSpawnerService
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService  _staticDataService;
        private readonly PlayerTransformDataHolder _playerTransformDataHolder;

        public PlayerSpawnerService(IInstantiator instantiator, IStaticDataService staticDataService, PlayerTransformDataHolder playerTransformDataHolder)
        {
            _instantiator = instantiator;
            _staticDataService = staticDataService;
            _playerTransformDataHolder = playerTransformDataHolder;
        }

        public Player SpawnPlayerAt(Vector3 position)
        {
            Player playerPrefab = _staticDataService.GetPlayerPrefab();

            Player instantiated = _instantiator.InstantiatePrefabForComponent<Player>(playerPrefab);
            instantiated.transform.position = position;
            
            _playerTransformDataHolder.Player = instantiated.transform;
            
            return instantiated;
        }
    }

    public class PlayerTransformDataHolder {
        public Transform Player { get; set; }
    }
}