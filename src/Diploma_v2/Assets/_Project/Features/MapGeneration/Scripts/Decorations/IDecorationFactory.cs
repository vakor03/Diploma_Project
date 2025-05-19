using UnityEngine;

namespace _Project.Features.MapGeneration.Decorations {
    public interface IDecorationFactory {
        void SpawnSmallFloorDecoration(Vector3 position, Transform parent);
        void SpawnBigFloorDecoration(Vector3 position, Transform parent);
        void SpawnCeilingDecoration(Vector3 position, Transform parent);
        void SpawnWallDecoration(Vector3 position, bool isLeftWall, Transform parent);
    }
} 