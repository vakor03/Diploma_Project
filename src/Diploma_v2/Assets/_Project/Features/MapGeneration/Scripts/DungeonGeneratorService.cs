using System.Collections.Generic;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.CA;
using _Project.Features.MapGeneration.Matrix;
using UnityEngine;

namespace _Project.Features.MapGeneration {
    public class DungeonGeneratorService : IDungeonGeneratorService {
        private readonly IMatrixFactory _matrixFactory;
        private readonly IMacroLayoutDungeonGenerationService _macroLayoutDungeonGenerationService;
        private readonly ICaveRoomCarveService _caveRoomCarveService;

        public DungeonGeneratorService(IMatrixFactory matrixFactory,
                                       IMacroLayoutDungeonGenerationService macroLayoutDungeonGenerationService,
                                       ICaveRoomCarveService caveRoomCarveService) {
            _matrixFactory = matrixFactory;
            _macroLayoutDungeonGenerationService = macroLayoutDungeonGenerationService;
            _caveRoomCarveService = caveRoomCarveService;
        }

        public Dungeon GenerateDungeon(DungeonGenerationConfiguration config) {
            Matrix<int> dungeonMatrix = _matrixFactory.CreateMatrix<int>(config.DungeonSize.x, config.DungeonSize.y);
            List<RectInt> rooms = _macroLayoutDungeonGenerationService.Generate(dungeonMatrix, config.BspDungeonGeneratorParams);
            foreach (RectInt room in rooms)
                _caveRoomCarveService.CarveRoom(dungeonMatrix, room, config.CACaveRoomParams);

            return new Dungeon() {
                Matrix = dungeonMatrix,
                Rooms = rooms,
            };
        }
    }
}