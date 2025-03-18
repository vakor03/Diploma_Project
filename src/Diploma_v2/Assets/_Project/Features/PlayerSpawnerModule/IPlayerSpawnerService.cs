using _Project.Features.PlayerModule;
using UnityEngine;

namespace _Project.Features.PlayerSpawnerModule
{
    public interface IPlayerSpawnerService
    {
        public Player SpawnPlayerAt(Vector3 position);
    }
}