using System.Collections.Generic;
using System.Linq;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.SeedModule;
using UnityEngine;

namespace _Project.Features.MapGeneration.Drukard {
    public class DrunkardCorridorGeneratorService : ICorridorGeneratorService
    {
        private readonly ISeedService _seedService;
        private const int FloorTile = 1;

        public DrunkardCorridorGeneratorService(ISeedService seedService) =>
            _seedService = seedService;

        public Tunnel CarveCorridor(Matrix<int> matrix, Vector2Int start, Vector2Int end, CorridorConfig config)
        {
            HashSet<Vector2Int> cells = new HashSet<Vector2Int>();
            System.Random rng = _seedService.GetRandom();
            Vector2Int current = start;
            CarveCellBlock(matrix, current, config.CorridorWidth, cells);

            while (!IsAtTarget(current, end))
            {
                Vector2Int step = StepTowardsTarget(current, end, rng, config.WanderChance);
                current = new Vector2Int(current.x + step.x, current.y + step.y);
                CarveCellBlock(matrix, current, config.CorridorWidth, cells);
            }

            return new Tunnel {
                Start = start,
                End = end,
                Cells = cells.ToList()
            };
        }

        private Vector2Int StepTowardsTarget(Vector2Int current, Vector2Int target, System.Random rng, float wanderChance) {
            int dx = target.x - current.x;
            int dy = target.y - current.y;
            bool shouldWander = rng.NextDouble() < wanderChance;

            if (shouldWander) {
                return GetRandomWanderStep(rng);
            }

            return GetBiasedStep(dx, dy);
        }

        private Vector2Int GetRandomWanderStep(System.Random rng) {
            bool horizontal = rng.NextDouble() < 0.5;
            int stepX = 0;
            int stepY = 0;

            if (horizontal) {
                stepX = rng.Next(0, 2) == 0 ? 1 : -1;
            } else {
                stepY = rng.Next(0, 2) == 0 ? 1 : -1;
            }

            return new Vector2Int(stepX, stepY);
        }

        private Vector2Int GetBiasedStep(int dx, int dy) {
            if (Mathf.Abs(dx) >= Mathf.Abs(dy)) {
                int xStep = dx > 0 ? 1 : -1;
                return new Vector2Int(xStep, 0);
            }

            int yStep = dy > 0 ? 1 : -1;
            return new Vector2Int(0, yStep);
        }


        private void CarveCellBlock(Matrix<int> matrix, Vector2Int center, int width, HashSet<Vector2Int> corridorCells) {
            int half = width / 2;
            int startOffset = -half;
            int endOffset;

            if (width % 2 == 0) {
                endOffset = half - 1;
            } else {
                endOffset = half;
            }

            for (int ox = startOffset; ox <= endOffset; ox++) {
                for (int oy = startOffset; oy <= endOffset; oy++) {
                    int x = center.x + ox;
                    int y = center.y + oy;

                    if (x >= 0 && y >= 0 && x < matrix.Width && y < matrix.Height) {
                        corridorCells.Add(new(x, y));
                        matrix[x, y] = FloorTile;
                    }
                }
            }
        }

        private bool IsAtTarget(Vector2Int current, Vector2Int target)
        {
            return current.x == target.x && current.y == target.y;
        }
    }

}
