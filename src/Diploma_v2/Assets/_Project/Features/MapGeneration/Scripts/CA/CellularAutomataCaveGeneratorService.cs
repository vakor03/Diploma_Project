using System.Collections.Generic;
using _Project.Features.MapGeneration.BSP;
using UnityEngine;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.SeedModule;

namespace _Project.Features.MapGeneration.CA {
    public class CellularAutomataCaveGeneratorService : ICaveRoomCarveService {
        private readonly ISeedService _seedService;

        private const int WALL = 0;
        private const int FLOOR = 1;

        private static readonly int[,] NeighborDirections8 = new int[8, 2] {
            { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 },
            { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }
        };

        private static readonly int[,] Directions4 = new int[4, 2] {
            { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }
        };

        public CellularAutomataCaveGeneratorService(ISeedService seedService) =>
            _seedService = seedService;

        public Room CarveRoom(Matrix<int> matrix, RectInt region, CAConfig config) {
            int[,] cells = InitializeCells(region, config);

            SmoothCells(ref cells, config);
            List<Vector2Int> largest = GetLargestRegion(cells);
            int[,] mask = CreateRegionMask(cells, largest);
            BlitToMatrix(matrix, region, mask, config.offsetFromBorders);
            
            Room room = new Room {
                PartitionBounds = region,
                RoomBounds = new RectInt(region.x + config.offsetFromBorders, region.y + config.offsetFromBorders, region.width - config.offsetFromBorders * 2, region.height - config.offsetFromBorders * 2),
                Cells = largest.ConvertAll(positionLocal => positionLocal + new Vector2Int(region.x + config.offsetFromBorders, region.y + config.offsetFromBorders)),
            };

            return room;
        }

        private int[,] InitializeCells(RectInt region, CAConfig config) {
            int offset = config.offsetFromBorders;
            int width = region.width - offset * 2;
            int height = region.height - offset * 2;
            int[,] cells = new int[width, height];
            System.Random rng = _seedService.GetRandom();

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    double r = rng.NextDouble();
                    cells[x, y] = (r < config.fillProbability) ? WALL : FLOOR;
                }
            }

            return cells;
        }

        private void SmoothCells(ref int[,] cells, CAConfig config) {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            for (int step = 0; step < config.steps; step++) {
                int[,] newCells = new int[width, height];

                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        int wallCount = CountWallNeighbors(cells, x, y, width, height);
                        if (wallCount > config.birthLimit)
                            newCells[x, y] = WALL;
                        else if (wallCount < config.deathLimit)
                            newCells[x, y] = FLOOR;
                        else
                            newCells[x, y] = cells[x, y];
                    }
                }

                cells = newCells;
            }
        }

        private List<Vector2Int> GetLargestRegion(int[,] cells) {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);
            bool[,] visited = new bool[width, height];
            List<List<Vector2Int>> regions = new List<List<Vector2Int>>();

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (!visited[x, y] && cells[x, y] == FLOOR) {
                        List<Vector2Int> region = FloodFill(cells, visited, x, y, width, height);
                        regions.Add(region);
                    }
                }
            }

            List<Vector2Int> largest = new List<Vector2Int>();
            int maxSize = 0;
            foreach (List<Vector2Int> region in regions) {
                int size = region.Count;
                if (size > maxSize) {
                    maxSize = size;
                    largest = region;
                }
            }

            return largest;
        }

        private int[,] CreateRegionMask(int[,] cells, List<Vector2Int> region) {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);
            int[,] mask = new int[width, height];

            foreach (Vector2Int cell in region)
                mask[cell.x, cell.y] = FLOOR;

            return mask;
        }

        private void BlitToMatrix(Matrix<int> matrix, RectInt region, int[,] mask, int offset) {
            int width = mask.GetLength(0);
            int height = mask.GetLength(1);

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++) {
                int globalX = region.x + offset + x;
                int globalY = region.y + offset + y;

                matrix[globalX, globalY] = mask[x, y] == FLOOR ? FLOOR : WALL;
            }
        }

        private int CountWallNeighbors(int[,] cells, int x, int y, int width, int height) {
            int count = 0;
            int length = NeighborDirections8.GetLength(0);
            for (int i = 0; i < length; i++) {
                int dx = NeighborDirections8[i, 0];
                int dy = NeighborDirections8[i, 1];
                int nx = x + dx;
                int ny = y + dy;
                if (nx < 0 || ny < 0 || nx >= width || ny >= height || cells[nx, ny] == WALL) {
                    count++;
                }
            }

            return count;
        }

        private List<Vector2Int> FloodFill(int[,] cells, bool[,] visited, int startX, int startY, int width, int height) {
            List<Vector2Int> region = new List<Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            visited[startX, startY] = true;
            queue.Enqueue(new Vector2Int(startX, startY));

            int dirCount = Directions4.GetLength(0);
            while (queue.Count > 0) {
                Vector2Int cell = queue.Dequeue();
                region.Add(cell);

                for (int i = 0; i < dirCount; i++) {
                    int dx = Directions4[i, 0];
                    int dy = Directions4[i, 1];
                    int nx = cell.x + dx;
                    int ny = cell.y + dy;
                    if (nx >= 0 && ny >= 0 && nx < width && ny < height && !visited[nx, ny] && cells[nx, ny] == FLOOR) {
                        visited[nx, ny] = true;
                        queue.Enqueue(new Vector2Int(nx, ny));
                    }
                }
            }

            return region;
        }
    }
}