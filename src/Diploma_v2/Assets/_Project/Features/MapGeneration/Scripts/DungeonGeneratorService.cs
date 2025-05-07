using System.Collections.Generic;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Matrix;
using UnityEngine;

namespace _Project.Features.MapGeneration {
    public class DungeonGeneratorService : IDungeonGeneratorService {
        private readonly IMatrixFactory _matrixFactory;
        private readonly IMacroLayoutDungeonGenerationService _macroLayoutDungeonGenerationService;

        public DungeonGeneratorService(IMatrixFactory matrixFactory,
                                       IMacroLayoutDungeonGenerationService macroLayoutDungeonGenerationService) {
            _matrixFactory = matrixFactory;
            _macroLayoutDungeonGenerationService = macroLayoutDungeonGenerationService;
        }

        public Dungeon GenerateDungeon(DungeonGenerationConfiguration config) {
            Matrix<int> dungeonMatrix = _matrixFactory.CreateMatrix<int>(config.DungeonSize.x, config.DungeonSize.y);
            List<RectInt> rooms = _macroLayoutDungeonGenerationService.Generate(dungeonMatrix, config.BspDungeonGeneratorParams);    

            return new Dungeon()
            {
                Matrix = dungeonMatrix,
                Rooms = rooms,
            };
        }
    }
}