using System.Collections.Generic;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.CA;
using _Project.Features.MapGeneration.Drukard;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.MapGeneration.RoomConnections;
using UnityEngine;

namespace _Project.Features.MapGeneration {
    public class DungeonGeneratorService : IDungeonGeneratorService {
        private readonly IMatrixFactory _matrixFactory;
        private readonly IMacroLayoutDungeonGenerationService _macroLayoutDungeonGenerationService;
        private readonly ICaveRoomCarveService _caveRoomCarveService;
        private readonly IRoomConnectorService _roomConnectorService;
        private readonly ICorridorGeneratorService _corridorGeneratorService;

        public DungeonGeneratorService(IMatrixFactory matrixFactory,
                                       IMacroLayoutDungeonGenerationService macroLayoutDungeonGenerationService,
                                       ICaveRoomCarveService caveRoomCarveService, IRoomConnectorService roomConnectorService, ICorridorGeneratorService corridorGeneratorService) {
            _matrixFactory = matrixFactory;
            _macroLayoutDungeonGenerationService = macroLayoutDungeonGenerationService;
            _caveRoomCarveService = caveRoomCarveService;
            _roomConnectorService = roomConnectorService;
            _corridorGeneratorService = corridorGeneratorService;
        }

        public Dungeon GenerateDungeon(DungeonGenerationConfiguration config) {
            Matrix<int> dungeonMatrix = _matrixFactory.CreateMatrix<int>(config.DungeonSize.x, config.DungeonSize.y);
            List<RectInt> macroGroup = _macroLayoutDungeonGenerationService.Generate(dungeonMatrix, config.BspDungeonGeneratorParams);
            List<Room> rooms = macroGroup.ConvertAll(room => _caveRoomCarveService.CarveRoom(dungeonMatrix, room, config.CACaveRoomParams));
            List<(Vector2Int start, Vector2Int end)> connections = _roomConnectorService.GetConnections(rooms);
            List<Tunnel> tunnels = connections.ConvertAll(tunnelEnds=>_corridorGeneratorService.CarveCorridor(dungeonMatrix, tunnelEnds.start, tunnelEnds.end, config.CorridorParams));

            return new Dungeon() {
                Matrix = dungeonMatrix,
                Rooms = rooms,
                Tunnels = tunnels,
            };
        }
    }
}