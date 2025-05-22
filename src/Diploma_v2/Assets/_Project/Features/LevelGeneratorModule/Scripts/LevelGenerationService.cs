using System.Collections.Generic;
using System.Linq;
using _Project.Features.EnemyModule;
using _Project.Features.LevelGeneratorModule.TilemapsBootstrap;
using _Project.Features.MapGeneration;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Decorations;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.MapGeneration.Tagging;
using _Project.Features.PlayerSpawnerModule;
using _Project.Features.SeedModule;
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
        private readonly IDecorationFactory _decorationFactory;
        private readonly ISeedService _seedService;
        private readonly BlockGroupsModel _blockGroupsModel;
        private readonly ISpawnPointsCalculatorService _spawnPointsCalculatorService;

        public LevelGenerationService(IDungeonGeneratorService dungeonGeneratorService, IStaticDataService staticData,
                                      PlayerSpawnPointsModel playerSpawnPointsModel, IInstantiator instantiator,
                                      ITilemapsBootstrapService tilemapsBootstrapService, ITilemapsService tilemapsService,
                                      TilemapsDataHolder tilemapsDataHolder, EnemySpawnPointsModel enemySpawnPointsModel,
                                      TilemapsCollidersConfiguration tilemapsCollidersConfiguration, IBlockGroupService blockGroupService,
                                      IDecorationFactory decorationFactory, ISeedService seedService, BlockGroupsModel blockGroupsModel,
                                      ISpawnPointsCalculatorService spawnPointsCalculatorService) {
            _dungeonGeneratorService = dungeonGeneratorService;
            _playerSpawnPointsModel = playerSpawnPointsModel;
            _instantiator = instantiator;
            _tilemapsBootstrapService = tilemapsBootstrapService;
            _tilemapsService = tilemapsService;
            _tilemapsDataHolder = tilemapsDataHolder;
            _enemySpawnPointsModel = enemySpawnPointsModel;
            _tilemapsCollidersConfiguration = tilemapsCollidersConfiguration;
            _blockGroupService = blockGroupService;
            _decorationFactory = decorationFactory;
            _seedService = seedService;
            _blockGroupsModel = blockGroupsModel;
            _spawnPointsCalculatorService = spawnPointsCalculatorService;
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
            SpawnFogOfWar(dungeon);

            // Create decorations parent
            Transform decorationsParent = new GameObject("Decorations").transform;
            
            SpawnDecorations(dungeon, decorationsParent);

            List<Vector2Int> positionsWithMacroTag = dungeon.Tags.GetPositionsWithMacroTag(MacroTag.Floor).Where(el=>
                dungeon.Matrix[el + Vector2Int.up] == BlockType.EmptySpace).ToList();
            _blockGroupService.GroupHorizontallyConnectedBlocks(positionsWithMacroTag);
            
            foreach (Vector2Int vector2Int in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.PlayerSpawnPoint))
                _playerSpawnPointsModel.SpawnPoints.Add(_spawnPointsCalculatorService.GetPositionFromTilemap(vector2Int, false));

            _spawnPointsCalculatorService.CalculateEnemySpawnPoints();
        }

        private void SpawnDecorations(Dungeon dungeon, Transform decorationsParent) {
            foreach (Vector2Int position in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.SmallFloorDecoration))
                _decorationFactory.SpawnSmallFloorDecoration(_spawnPointsCalculatorService.GetPositionFromTilemap(position, false), decorationsParent);

            foreach (Vector2Int position in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.BigFloorDecoration))
                _decorationFactory.SpawnBigFloorDecoration(_spawnPointsCalculatorService.GetPositionFromTilemap(position, true), decorationsParent);

            foreach (Vector2Int position in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.CeilingDecoration))
                _decorationFactory.SpawnCeilingDecoration(_spawnPointsCalculatorService.GetPositionFromTilemap(position, false), decorationsParent);

            foreach (Vector2Int position in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.WallDecoration)) {
                bool isLeftWall = dungeon.Matrix[position + Vector2Int.left] == BlockType.Wall;
                _decorationFactory.SpawnWallDecoration(_spawnPointsCalculatorService.GetPositionFromTilemap(position, true), isLeftWall, decorationsParent);
            }
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
            HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
            
            // Add existing wall positions
            Vector2Int[] floorIndices = matrix.GetAllIndices(el => el == BlockType.Wall).ToArray();
            wallPositions.UnionWith(floorIndices);

            // Add wall tiles in a 10-block border around the matrix
            int width = matrix.Width;
            int height = matrix.Height;
            
            // Top and bottom borders
            for (int x = -10; x < width + 10; x++) {
                for (int y = -10; y < 0; y++) {
                    wallPositions.Add(new Vector2Int(x, y));
                }
                for (int y = height; y < height + 10; y++) {
                    wallPositions.Add(new Vector2Int(x, y));
                }
            }
            
            // Left and right borders
            for (int y = 0; y < height; y++) {
                for (int x = -10; x < 0; x++) {
                    wallPositions.Add(new Vector2Int(x, y));
                }
                for (int x = width; x < width + 10; x++) {
                    wallPositions.Add(new Vector2Int(x, y));
                }
            }

            // Convert HashSet to array and create wall tiles
            Vector2Int[] wallPositionsArray = wallPositions.ToArray();
            TileBase[] tileBases = Enumerable.Repeat(_levelConfiguration.TilesConfiguration.FloorTile, wallPositionsArray.Length).ToArray();

            _tilemapsService.SetTiles(TilemapType.Background,
                wallPositionsArray,
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

        private void SpawnFogOfWar(Dungeon dungeon) {
            HashSet<Vector2Int> fogPositions = new HashSet<Vector2Int>();
            
            // Add fog tiles on wall positions
            Vector2Int[] wallIndices = dungeon.Matrix.GetAllIndices(el => el == BlockType.Wall).ToArray();
            fogPositions.UnionWith(wallIndices);

            // Add fog tiles in a 10-block border around the matrix
            int width = dungeon.Matrix.Width;
            int height = dungeon.Matrix.Height;
            
            // Top and bottom borders
            for (int x = -10; x < width + 10; x++) {
                for (int y = -10; y < 0; y++) {
                    fogPositions.Add(new Vector2Int(x, y));
                }
                for (int y = height; y < height + 10; y++) {
                    fogPositions.Add(new Vector2Int(x, y));
                }
            }
            
            // Left and right borders
            for (int y = 0; y < height; y++) {
                for (int x = -10; x < 0; x++) {
                    fogPositions.Add(new Vector2Int(x, y));
                }
                for (int x = width; x < width + 10; x++) {
                    fogPositions.Add(new Vector2Int(x, y));
                }
            }

            // Convert HashSet to array and create fog tiles
            Vector2Int[] fogPositionsArray = fogPositions.ToArray();
            TileBase[] fogTiles = Enumerable.Repeat(_levelConfiguration.TilesConfiguration.FogOfWarTile, fogPositionsArray.Length).ToArray();

            _tilemapsService.SetTiles(TilemapType.FogOfWar, fogPositionsArray, fogTiles);
        }
    }
}