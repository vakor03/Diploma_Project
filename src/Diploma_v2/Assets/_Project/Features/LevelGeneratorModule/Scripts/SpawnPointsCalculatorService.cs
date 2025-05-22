using System.Collections.Generic;
using _Project.Features.EnemyModule;
using _Project.Features.EnemyModule.EnemyPool;
using _Project.Features.LevelGeneratorModule.TilemapsBootstrap;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.SeedModule;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace _Project.Features.LevelGeneratorModule
{
    public interface ISpawnPointsCalculatorService
    {
        void CalculateEnemySpawnPoints();
        Vector3 GetPositionFromTilemap(Vector2Int tilePosition, bool centerOfTile);
    }

    public class SpawnPointsCalculatorService : ISpawnPointsCalculatorService
    {
        private readonly ISeedService _seedService;
        private readonly TilemapsDataHolder _tilemapsDataHolder;
        private readonly BlockGroupsModel _blockGroupsModel;
        private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

        public SpawnPointsCalculatorService(ISeedService seedService, TilemapsDataHolder tilemapsDataHolder, BlockGroupsModel blockGroupsModel, EnemySpawnPointsModel enemySpawnPointsModel)
        {
            _seedService = seedService;
            _tilemapsDataHolder = tilemapsDataHolder;
            _blockGroupsModel = blockGroupsModel;
            _enemySpawnPointsModel = enemySpawnPointsModel;
        }

        public void CalculateEnemySpawnPoints()
        {
            _enemySpawnPointsModel.SpawnPoints.Clear();
            var random = _seedService.GetRandom();

            foreach (BlockGroup group in _blockGroupsModel.Groups)
            {
                bool shouldSpawnTurret = group.Blocks.Count > 6 && random.NextDouble() < 1f;
                
                if (shouldSpawnTurret)
                {
                    Vector2Int leftmostPos = group.Blocks[0];
                    Vector2Int rightmostPos = group.Blocks[group.Blocks.Count - 1];
                    
                    bool spawnOnRight = random.Next(0, 2) == 1;
                    Vector2Int turretPos = spawnOnRight ? rightmostPos : leftmostPos;
                    
                    Vector2Int centerPos = group.Blocks[group.Blocks.Count / 2];
                    bool faceRight = turretPos.x < centerPos.x; // Face towards center
                    
                    Vector3 worldPos = GetPositionFromTilemap(turretPos, false);
                    _enemySpawnPointsModel.SpawnPoints.Add(new EnemySpawnData(worldPos, EnemyType.OnePlaceGuardRobot, faceRight));
                    
                    group.Blocks.Remove(turretPos);
                }

                List<Vector2Int> availablePositions = new List<Vector2Int>(group.Blocks);
                for (int i = 0; i < 2 && availablePositions.Count > 0; i++)
                {
                    int randomIndex = random.Next(0, availablePositions.Count);
                    Vector2Int spawnPos = availablePositions[randomIndex];
                    availablePositions.RemoveAt(randomIndex);

                    Vector3 worldPos = GetPositionFromTilemap(spawnPos, false);
                    // bool faceRight = random.Next(0, 2) == 1;
                    bool faceRight = true;
                    EnemyType enemyType = ChooseRandom(new List<EnemyType>() { EnemyType.BigGuardRobot , EnemyType.ShadowOfStorms});

                    _enemySpawnPointsModel.SpawnPoints.Add(new EnemySpawnData(worldPos, enemyType, faceRight));
                    // return;
                }
            }
        }
        
        private EnemyType ChooseRandom(List<EnemyType> enemyTypes)
        {
            int randomIndex = _seedService.GetRandom().Next(0, enemyTypes.Count);
            return enemyTypes[randomIndex];
        }

        public Vector3 GetPositionFromTilemap(Vector2Int tilePosition, bool centerOfTile)
        {
            if (!_tilemapsDataHolder.TryGetTilemap(TilemapType.Background, out Tilemap tilemap))
                Debug.LogException(new System.Exception($"Tilemap {TilemapType.Background} not found"));

            Vector3Int cellPosition = new Vector3Int(tilePosition.x, tilePosition.y, 0);
            Vector3 worldPosition = tilemap.CellToWorld(cellPosition);

            if (centerOfTile)
            {
                Vector3 cellSize = tilemap.cellSize;
                worldPosition += new Vector3(cellSize.x * 0.5f, cellSize.y * 0.5f, 0);
            }

            return worldPosition;
        }
    }
} 