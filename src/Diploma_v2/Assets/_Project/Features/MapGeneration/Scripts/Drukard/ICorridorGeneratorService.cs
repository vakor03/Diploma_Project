using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Matrix;
using UnityEngine;

namespace _Project.Features.MapGeneration.Drukard {
    public interface ICorridorGeneratorService {
        public Tunnel CarveCorridor(Matrix<BlockType> matrix, Vector2Int start, Vector2Int end, CorridorConfig config);
    }
}