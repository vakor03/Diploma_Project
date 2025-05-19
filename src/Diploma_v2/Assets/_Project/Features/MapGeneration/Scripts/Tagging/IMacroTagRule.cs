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

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags) {
            if (Context.Matrix[position] == BlockType.Wall || Context.Matrix[position] == BlockType.Platform)
                return false;

            return Context.Matrix[position + Vector2Int.down] == BlockType.Wall ||
                   dungeonTags.GetMacroTag(position + Vector2Int.down) == MacroTag.Platform;
        }

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags) =>
            Context.Matrix[position + Vector2Int.down] == BlockType.Wall ||
            dungeonTags.GetMacroTag(position + Vector2Int.down) == MacroTag.Platform;
    }

    public class PlatformMacroTagRule : IMacroTagRule {
        public int MaxCount => int.MaxValue;
        public Dungeon Context { get; set; }
        public MacroTag MacroTag => MacroTag.Platform;
        public GlobalPlaceTagFilter GlobalPlaceTagFilter { get; } = new();

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags) {
            if (Context.Matrix[position] == BlockType.Wall)
                return false;

            if ((position.y - room.RoomBounds.yMin) % 5 == 0 && position.y != room.RoomBounds.yMin &&
                (position.y + 3) <= room.RoomBounds.yMax)
                return true;
            return false;
        }

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags) {
            if (Context.Matrix[position] == BlockType.Wall)
                return false;

            // Only allow platforms in vertical tunnels
            if (globalPlaceTag != GlobalPlaceTag.TunnelVertical)
                return false;

            int yMin = Mathf.Min(tunnel.Start.y, tunnel.End.y);
            if ((position.y - yMin) % 5 == 0)
                return true;
            return false;
        }
    }
}