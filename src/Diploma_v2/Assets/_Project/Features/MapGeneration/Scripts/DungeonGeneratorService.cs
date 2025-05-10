using System.Collections.Generic;
using System.Linq;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.CA;
using _Project.Features.MapGeneration.Drukard;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.MapGeneration.RoomConnections;
using _Project.Features.MapGeneration.Tagging;
using _Project.Global.Collections;
using UnityEngine;

namespace _Project.Features.MapGeneration {
    public class DungeonGeneratorService : IDungeonGeneratorService {
        private readonly IMatrixFactory _matrixFactory;
        private readonly IMacroLayoutDungeonGenerationService _macroLayoutDungeonGenerationService;
        private readonly ICaveRoomCarveService _caveRoomCarveService;
        private readonly IRoomConnectorService _roomConnectorService;
        private readonly ICorridorGeneratorService _corridorGeneratorService;
        private readonly IDungeonTagService _dungeonTagService;

        public DungeonGeneratorService(IMatrixFactory matrixFactory,
                                       IMacroLayoutDungeonGenerationService macroLayoutDungeonGenerationService,
                                       ICaveRoomCarveService caveRoomCarveService, IRoomConnectorService roomConnectorService,
                                       ICorridorGeneratorService corridorGeneratorService, IDungeonTagService dungeonTagService) {
            _matrixFactory = matrixFactory;
            _macroLayoutDungeonGenerationService = macroLayoutDungeonGenerationService;
            _caveRoomCarveService = caveRoomCarveService;
            _roomConnectorService = roomConnectorService;
            _corridorGeneratorService = corridorGeneratorService;
            _dungeonTagService = dungeonTagService;
        }

        public Dungeon GenerateDungeon(DungeonGenerationConfiguration config) {
            Matrix<BlockType> dungeonMatrix = _matrixFactory.CreateMatrix<BlockType>(config.DungeonSize.x, config.DungeonSize.y);
            List<RectInt> macroGroup = _macroLayoutDungeonGenerationService.Generate(dungeonMatrix, config.BspDungeonGeneratorParams);
            List<Room> rooms = macroGroup.Select(room => _caveRoomCarveService.CarveRoom(dungeonMatrix, room, config.CACaveRoomParams))
                .Where(room => room.Cells.Count > 0).ToList();
            foreach (Room room in rooms) {
                new HorizontalSurfaceSmoother().SmoothHorizontalSurfaces(dungeonMatrix, room);
            }
            List<(Vector2Int start, Vector2Int end)> connections = _roomConnectorService.GetConnections(rooms);
            List<Tunnel> tunnels = connections.ConvertAll(tunnelEnds =>
                _corridorGeneratorService.CarveCorridor(dungeonMatrix, tunnelEnds.start, tunnelEnds.end, config.CorridorParams));

            Dungeon dungeon = new Dungeon() {
                Matrix = dungeonMatrix,
                Rooms = rooms,
                Tunnels = tunnels,
            };

            DungeonTags dungeonTags = _dungeonTagService.TagAllRegions(dungeon, GlobalPlaceTagRules(), MacroTagRules(), MicroTagRules());

            dungeon.Tags = dungeonTags;

            return dungeon;
        }

        private PriorityList<IGlobalPlaceTagRule> GlobalPlaceTagRules() {
            PriorityList<IGlobalPlaceTagRule> rules = new();
            rules.Add(new InitialRoomGlobalPlaceTagRule(), 10);
            rules.Add(new DefaultRoomGlobalPlaceTagRule(), 0);
            rules.Add(new DefaultTunnelGlobalPlaceTagRule(), 0);
            return rules;
        }

        private PriorityList<IMicroTagRule> MicroTagRules() {
            PriorityList<IMicroTagRule> rules = new();
            rules.Add(new PlatformMicroTagRule(), 11);
            rules.Add(new PlayerSpawnMicroTagRule(), 10);
            rules.Add(new EnemySpawnMicroTagRule(), 9);
            return rules;
        }

        private PriorityList<IMacroTagRule> MacroTagRules() {
            PriorityList<IMacroTagRule> rules = new();
            rules.Add(new FloorMacroTagRule(), 10);
            return rules;
        }
    }

    public enum BlockType {
        Wall = 0,
        EmptySpace = 1,
    }
}