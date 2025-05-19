using _Project.Features.SeedModule;
using UnityEngine;

namespace _Project.Features.MapGeneration.Decorations {
    public class DecorationFactory : IDecorationFactory {
        private readonly DecorationConfiguration _config;
        private readonly ISeedService _seedService;

        public DecorationFactory(DecorationConfiguration config, ISeedService seedService) {
            _config = config;
            _seedService = seedService;
        }

        public void SpawnSmallFloorDecoration(Vector3 position, Transform parent) {
            if (_config.smallFloorDecorations.prefabs.Count == 0) return;

            GameObject prefab = GetRandomPrefab(_config.smallFloorDecorations.prefabs);
            Vector3 spawnPosition = position + Vector3.up * _config.decorationHeightOffset;
            SpawnDecoration(prefab, spawnPosition, Quaternion.identity, parent);
        }

        public void SpawnBigFloorDecoration(Vector3 position, Transform parent) {
            if (_config.bigFloorDecorations.prefabs.Count == 0) return;

            GameObject prefab = GetRandomPrefab(_config.bigFloorDecorations.prefabs);
            Vector3 spawnPosition = position + Vector3.up * _config.decorationHeightOffset;
            SpawnDecoration(prefab, spawnPosition, Quaternion.identity, parent);
        }

        public void SpawnCeilingDecoration(Vector3 position, Transform parent) {
            if (_config.ceilingDecorations.prefabs.Count == 0) return;

            GameObject prefab = GetRandomPrefab(_config.ceilingDecorations.prefabs);
            Vector3 spawnPosition = position;
            SpawnDecoration(prefab, spawnPosition, Quaternion.identity, parent);
        }

        public void SpawnWallDecoration(Vector3 position, bool isLeftWall, Transform parent) {
            if (_config.wallDecorations.prefabs.Count == 0) return;

            GameObject prefab = GetRandomPrefab(_config.wallDecorations.prefabs);
            Vector3 spawnPosition = position + Vector3.up * _config.decorationHeightOffset;
            
            // Adjust position based on wall side
            if (isLeftWall) {
                spawnPosition += Vector3.right * _config.wallDecorationDepth;
            } else {
                spawnPosition += Vector3.left * _config.wallDecorationDepth;
            }

            // Rotate to face the wall
            Quaternion rotation = Quaternion.Euler(0f, isLeftWall ? 90f : -90f, 0f);
            SpawnDecoration(prefab, spawnPosition, rotation, parent);
        }

        private GameObject GetRandomPrefab(System.Collections.Generic.List<GameObject> prefabs) {
            int index = _seedService.GetRandom().Next(prefabs.Count);
            return prefabs[index];
        }

        private void SpawnDecoration(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
            Object.Instantiate(prefab, position, rotation, parent);
        }
    }
} 