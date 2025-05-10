using _Project.Features.MapGeneration.BSP;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging {
    public interface IMacroTagRule {
        public int MaxCount { get; }
        public Dungeon Context { set; }
        public MacroTag MacroTag { get; }
        public GlobalPlaceTagFilter GlobalPlaceTagFilter { get; }
        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags);
        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags);
    }
    
    public class FloorMacroTagRule : IMacroTagRule {
        public int MaxCount => int.MaxValue;
        public Dungeon Context { get; set; }
        public MacroTag MacroTag => MacroTag.Floor;
        public GlobalPlaceTagFilter GlobalPlaceTagFilter => new();

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags) =>
            !room.Cells.Contains(position + Vector2Int.down);

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags) =>
            !tunnel.Cells.Contains(position + Vector2Int.down);
    }
}