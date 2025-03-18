using _Project.Features.SeedModule;
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
        private readonly PlayerSpawnPointMarker[] _spawnPointMarkers;

        public PlayerSpawnPointsService(PlayerSpawnPointMarker[] spawnPointMarkers, ISeedService seedService)
        {
            _spawnPointMarkers = spawnPointMarkers;
            _seedService = seedService;
        }

        public Vector3 GetPlayerSpawnPoint()
        {
            int randomIndex = _seedService.GetRandom().Next(0, _spawnPointMarkers.Length);
            return _spawnPointMarkers[randomIndex].transform.position;
        }
    }
}