using _Project.Features.MapGeneration;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.MapGeneration.Tagging;
using _Project.Features.PlayerSpawnerModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;

namespace _Project.Features.LevelGeneratorModule
{
    public class LevelGenerationService : ILevelGenerationService
    {
        private readonly IDungeonGeneratorService _dungeonGeneratorService;
        private readonly LevelConfiguration _levelConfiguration;
        private readonly PlayerSpawnPointsModel _playerSpawnPointsModel;
        
        public LevelGenerationService(IDungeonGeneratorService dungeonGeneratorService, IStaticDataService staticData, PlayerSpawnPointsModel playerSpawnPointsModel) {
            _dungeonGeneratorService = dungeonGeneratorService;
            _playerSpawnPointsModel = playerSpawnPointsModel;
            _levelConfiguration = staticData.GetLevelConfiguration();
        }

        public void Generate()
        {
            DungeonGenerationConfiguration generationConfiguration = _levelConfiguration.DungeonGenerationConfiguration;
            Dungeon dungeon = _dungeonGeneratorService.GenerateDungeon(generationConfiguration);
            
            SpawnSquaresFromMatrix(dungeon.Matrix);
            foreach (Vector2Int vector2Int in dungeon.Tags.TaggedSubSpaces[SubSpaceTag.PlayerSpawnPoint])
                _playerSpawnPointsModel.SpawnPoints.Add((Vector2)vector2Int);
        }
            
        private void SpawnSquaresFromMatrix(Matrix<int> matrix)
        {
            for (int row = 0; row < matrix.Height; row++)
            for (int col = 0; col < matrix.Width; col++)
                if (matrix[row, col] == 0)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector2(row, col);
                }
        }
    }
}