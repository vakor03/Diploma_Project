using _Project.Features.MapGeneration.BSP;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging {
    public interface IMicroTagRule {
        public int GlobalMaxCount { get; }
        public int LocalMaxCount { get; }
        public Dungeon Context { set; }
        public MicroTag MicroTag { get; }
        public GlobalPlaceTagFilter GlobalPlaceTagFilter { get; }
        public MacroTagFilter MacroTagFilter { get; }

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags);

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags);
    }
}