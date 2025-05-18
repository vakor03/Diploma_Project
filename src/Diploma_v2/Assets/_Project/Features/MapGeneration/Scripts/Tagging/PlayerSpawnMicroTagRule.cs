using _Project.Features.MapGeneration.BSP;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging {
    public class PlayerSpawnMicroTagRule : IMicroTagRule {
        public int GlobalMaxCount => 1;
        public int LocalMaxCount => 1;
        public Dungeon Context { get; set; }
        public MicroTag MicroTag => MicroTag.PlayerSpawnPoint;

        public GlobalPlaceTagFilter GlobalPlaceTagFilter => new GlobalPlaceTagFilter(GlobalPlaceTag.InitialRoom);

        public MacroTagFilter MacroTagFilter => new MacroTagFilter(MacroTag.Floor);

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) =>
            true;

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) =>
            false;
    }
    
    public class EnemySpawnMicroTagRule : IMicroTagRule {
        public int GlobalMaxCount => int.MaxValue;
        public int LocalMaxCount => 3;
        public Dungeon Context { get; set; }
        public MicroTag MicroTag => MicroTag.EnemySpawnPoint;

        public GlobalPlaceTagFilter GlobalPlaceTagFilter => new GlobalPlaceTagFilter(GlobalPlaceTag.DefaultRoom);

        public MacroTagFilter MacroTagFilter => new MacroTagFilter(MacroTag.Floor);

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) =>
            true;

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) =>
            true;
    }
    
    // public class PlatformMicroTagRule : IMicroTagRule {
    //     public int GlobalMaxCount => int.MaxValue;
    //     public int LocalMaxCount => int.MaxValue;
    //     public Dungeon Context { get; set; }
    //     public MicroTag MicroTag => MicroTag.Platform;
    //
    //     public GlobalPlaceTagFilter GlobalPlaceTagFilter => new GlobalPlaceTagFilter();
    //
    //     public MacroTagFilter MacroTagFilter => new();
    //
    //     public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
    //                                    DungeonTags dungeonTags) {
    //         if ((position.y - room.RoomBounds.yMin) % 5 == 0 && position.y != room.RoomBounds.yMin  && (position.y + 3) <= room.RoomBounds.yMax)
    //             return true;
    //         return false;
    //     }
    //
    //     public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
    //                                    DungeonTags dungeonTags) {
    //         int yMin = Mathf.Min(tunnel.Start.y, tunnel.End.y);
    //         if ((position.y - yMin) % 5 == 0)
    //             return true;
    //         return false;
    //     }
    // }
}