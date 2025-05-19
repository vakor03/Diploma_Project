using System.Collections.Generic;
using System.Linq;
using _Project.Features.LevelGeneratorModule.TilemapsBootstrap;
using _Project.Features.MapGeneration;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.MapGeneration.Tagging;
using _Project.Features.PlayerSpawnerModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace _Project.Features.LevelGeneratorModule {
    public class LevelGenerationService : ILevelGenerationService {
        private readonly IDungeonGeneratorService _dungeonGeneratorService;
        private readonly LevelConfiguration _levelConfiguration;
        private readonly PlayerSpawnPointsModel _playerSpawnPointsModel;
        private readonly IInstantiator _instantiator;
        private readonly ITilemapsBootstrapService _tilemapsBootstrapService;
        private readonly ITilemapsService _tilemapsService;
        private readonly TilemapsDataHolder _tilemapsDataHolder;
        private readonly EnemySpawnPointsModel _enemySpawnPointsModel;
        private readonly TilemapsCollidersConfiguration _tilemapsCollidersConfiguration;
        private readonly IBlockGroupService _blockGroupService;

        public LevelGenerationService(IDungeonGeneratorService dungeonGeneratorService, IStaticDataService staticData,
                                      PlayerSpawnPointsModel playerSpawnPointsModel, IInstantiator instantiator,
                                      ITilemapsBootstrapService tilemapsBootstrapService, ITilemapsService tilemapsService,
                                      TilemapsDataHolder tilemapsDataHolder, EnemySpawnPointsModel enemySpawnPointsModel,
                                      TilemapsCollidersConfiguration tilemapsCollidersConfiguration, IBlockGroupService blockGroupService) {
            _dungeonGeneratorService = dungeonGeneratorService;
            _playerSpawnPointsModel = playerSpawnPointsModel;
            _instantiator = instantiator;
            _tilemapsBootstrapService = tilemapsBootstrapService;
            _tilemapsService = tilemapsService;
            _tilemapsDataHolder = tilemapsDataHolder;
            _enemySpawnPointsModel = enemySpawnPointsModel;
            _tilemapsCollidersConfiguration = tilemapsCollidersConfiguration;
            _blockGroupService = blockGroupService;
            _levelConfiguration = staticData.GetLevelConfiguration();
        }

        public void Generate() {
            _tilemapsBootstrapService.BootstrapTilemaps();

            DungeonGenerationConfiguration generationConfiguration = _levelConfiguration.DungeonGenerationConfiguration;
            Dungeon dungeon = _dungeonGeneratorService.GenerateDungeon(generationConfiguration);

            SpawnTilesForDungeon(dungeon.Matrix);
            SpawnPlatformsForDungeon(dungeon);
            SpawnCollidersForFloor(dungeon);
            SpawnCollidersForPlatforms(dungeon);
            
            _blockGroupService.GroupHorizontallyConnectedBlocks(dungeon.Tags.GetPositionsWithMacroTag(MacroTag.Floor));
            
            foreach (Vector2Int vector2Int in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.PlayerSpawnPoint))
                _playerSpawnPointsModel.SpawnPoints.Add(GetPositionFromTilemap(vector2Int, false));

            foreach (Vector2Int vector2Int in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.EnemySpawnPoint))
                _enemySpawnPointsModel.SpawnPoints.Add(GetPositionFromTilemap(vector2Int, false));
        }

        private void SpawnCollidersForFloor(Dungeon dungeon) {
            SpawnCollidersForMatrix(dungeon.Matrix.GetAllIndices(el => el == BlockType.Wall), _tilemapsCollidersConfiguration.FloorTilemapCollider);
        }

        private void SpawnCollidersForPlatforms(Dungeon dungeon) {
            Vector2Int[] platformIndices = dungeon.Tags.GetPositionsWithMacroTag(MacroTag.Platform).ToArray();

            SpawnCollidersForMatrix(platformIndices, _tilemapsCollidersConfiguration.PlatformTilemapCollider);
        }

        private void SpawnPlatformsForDungeon(Dungeon dungeon) {
            Vector2Int[] platformIndices = dungeon.Tags.GetPositionsWithMacroTag(MacroTag.Platform).ToArray();
            TileBase[] tileBases = Enumerable.Repeat(_levelConfiguration.TilesConfiguration.PlatformTile, platformIndices.Length)
                .ToArray();

            _tilemapsService.SetTiles(TilemapType.Platform,
                platformIndices,
                tileBases);
        }

        private Vector2 GetPositionFromTilemap(Vector2Int tilePosition, bool centerOfTile) {
            if (!_tilemapsDataHolder.TryGetTilemap(TilemapType.Background, out Tilemap tilemap))
                Debug.LogException(new System.Exception($"Tilemap {TilemapType.Background} not found"));

            Vector3Int cellPosition = new Vector3Int(tilePosition.x, tilePosition.y, 0);
            Vector3 worldPosition = tilemap.CellToWorld(cellPosition);

            if (centerOfTile) {
                Vector3 cellSize = tilemap.cellSize;
                worldPosition += new Vector3(cellSize.x * 0.5f, cellSize.y * 0.5f, 0);
            }

            return worldPosition;
        }

        private void SpawnTilesForDungeon(Matrix<BlockType> matrix) {
            Vector2Int[] floorIndices = matrix.GetAllIndices(el => el == BlockType.Wall).ToArray();
            TileBase[] tileBases = Enumerable.Repeat(_levelConfiguration.TilesConfiguration.FloorTile, floorIndices.Length).ToArray();

            _tilemapsService.SetTiles(TilemapType.Background,
                floorIndices,
                tileBases);
        }

        private void SpawnCollidersForMatrix(IEnumerable<Vector2Int> indices, TilemapColliderConfiguration collidersConfiguration) {
            CompositeCollider2D parentCollider =
                _instantiator.InstantiatePrefabForComponent<CompositeCollider2D>(collidersConfiguration.CompositeCollider);
            parentCollider.gameObject.layer = (int)collidersConfiguration.Layer;
            parentCollider.generationType = CompositeCollider2D.GenerationType.Manual;

            _tilemapsDataHolder.TryGetTilemap(TilemapType.Background, out Tilemap tilemap);
            Vector3 cellSize = tilemap.cellSize;

            foreach (Vector2Int wallPos in indices) {
                Vector2 worldPos = GetPositionFromTilemap(wallPos, false);
                BoxCollider2D boxCollider = _instantiator.InstantiatePrefabForComponent<BoxCollider2D>(
                    collidersConfiguration.SingleCollider, worldPos, Quaternion.identity, parentCollider.transform);

                boxCollider.size *= new Vector2(cellSize.x, cellSize.y);
            }

            parentCollider.GenerateGeometry();
            parentCollider.generationType = CompositeCollider2D.GenerationType.Synchronous;
        }
    }
}