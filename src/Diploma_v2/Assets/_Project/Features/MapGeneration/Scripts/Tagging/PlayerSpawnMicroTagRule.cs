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
            false;
    }
}