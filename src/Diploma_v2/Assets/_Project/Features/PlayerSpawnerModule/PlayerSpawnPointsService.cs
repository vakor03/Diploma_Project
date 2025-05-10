using System.Collections.Generic;
using _Project.Features.SeedModule;
using _Project.Scripts.Infrastructure;
using UnityEngine;

namespace _Project.Features.PlayerSpawnerModule
{
    public interface IPlayerSpawnPointsService
    {
        public Vector3 GetPlayerSpawnPoint();
    }

    public class PlayerSpawnPointsService : IPlayerSpawnPointsService
    {
        private readonly ISeedService  _seedService;
        private readonly PlayerSpawnPointsModel _playerSpawnPointsModel;

        public PlayerSpawnPointsService(ISeedService seedService, PlayerSpawnPointsModel playerSpawnPointsModel) {
            _seedService = seedService;
            _playerSpawnPointsModel = playerSpawnPointsModel;
        }

        public Vector3 GetPlayerSpawnPoint()
        {
            int randomIndex = _seedService.GetRandom().Next(0, _playerSpawnPointsModel.SpawnPoints.Count);
            return _playerSpawnPointsModel.SpawnPoints[randomIndex];
        }
    }

    public class PlayerSpawnPointsModel : IModel {
        public List<Vector3> SpawnPoints { get; set; } = new();
    }
    
    public class EnemySpawnPointsModel : IModel {
        public List<Vector3> SpawnPoints { get; set; } = new();
    }
}