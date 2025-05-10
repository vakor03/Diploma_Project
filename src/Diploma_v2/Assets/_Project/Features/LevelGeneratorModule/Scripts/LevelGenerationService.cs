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

        public LevelGenerationService(IDungeonGeneratorService dungeonGeneratorService, IStaticDataService staticData,
                                      PlayerSpawnPointsModel playerSpawnPointsModel, IInstantiator instantiator,
                                      ITilemapsBootstrapService tilemapsBootstrapService, ITilemapsService tilemapsService, TilemapsDataHolder tilemapsDataHolder, EnemySpawnPointsModel enemySpawnPointsModel) {
            _dungeonGeneratorService = dungeonGeneratorService;
            _playerSpawnPointsModel = playerSpawnPointsModel;
            _instantiator = instantiator;
            _tilemapsBootstrapService = tilemapsBootstrapService;
            _tilemapsService = tilemapsService;
            _tilemapsDataHolder = tilemapsDataHolder;
            _enemySpawnPointsModel = enemySpawnPointsModel;
            _levelConfiguration = staticData.GetLevelConfiguration();
        }

        public void Generate() {
            _tilemapsBootstrapService.BootstrapTilemaps();

            DungeonGenerationConfiguration generationConfiguration = _levelConfiguration.DungeonGenerationConfiguration;
            Dungeon dungeon = _dungeonGeneratorService.GenerateDungeon(generationConfiguration);

            SpawnTilesForDungeon(dungeon.Matrix);
            foreach (Vector2Int vector2Int in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.PlayerSpawnPoint))
                _playerSpawnPointsModel.SpawnPoints.Add(GetPositionFromTilemap(vector2Int, true));
            
            foreach (Vector2Int vector2Int in dungeon.Tags.GetPositionsWithMicroTag(MicroTag.EnemySpawnPoint))
                _enemySpawnPointsModel.SpawnPoints.Add(GetPositionFromTilemap(vector2Int, true));
        }

        private Vector2 GetPositionFromTilemap(Vector2Int tilePosition, bool centerOfTile) {
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

        private void SpawnTilesForDungeon(Matrix<int> matrix) {
            Vector2Int[] floorIndices = matrix.GetAllIndices(el => el == 0).ToArray();
            TileBase[] tileBases = Enumerable.Repeat(_levelConfiguration.TilesConfiguration.FloorTile, floorIndices.Length).ToArray();

            _tilemapsService.SetTiles(TilemapType.Background,
                floorIndices,
                tileBases);
        }
    }
}