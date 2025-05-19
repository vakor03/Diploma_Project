using _Project.Features.MapGeneration.BSP;
using Unity.Mathematics.Geometry;
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
    
    public class PlatformMicroTagRule : IMicroTagRule {
        public int GlobalMaxCount => int.MaxValue;
        public int LocalMaxCount => int.MaxValue;
        public Dungeon Context { get; set; }
        public MicroTag MicroTag => MicroTag.Platform;

        public GlobalPlaceTagFilter GlobalPlaceTagFilter => new GlobalPlaceTagFilter();

        public MacroTagFilter MacroTagFilter => new();

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) {
            if ((position.y - room.RoomBounds.yMin) % 5 == 0 && position.y != room.RoomBounds.yMin  && (position.y + 3) <= room.RoomBounds.yMax)
                return true;
            return false;
        }

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) {
            int yMin = Mathf.Min(tunnel.Start.y, tunnel.End.y);
            if ((position.y - yMin) % 5 == 0)
                return true;
            return false;
        }
    }

    public class SmallFloorDecorationMicroTagRule : IMicroTagRule {
        public int GlobalMaxCount => int.MaxValue;
        public int LocalMaxCount => 60;
        public Dungeon Context { get; set; }
        public MicroTag MicroTag => MicroTag.SmallFloorDecoration;

        public GlobalPlaceTagFilter GlobalPlaceTagFilter => new();

        public MacroTagFilter MacroTagFilter => new MacroTagFilter(MacroTag.Floor);

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) {
            // Check if the cell is a valid floor cell
            if (Context.Matrix[position] != BlockType.EmptySpace ||
                dungeonTags.GetMacroTag(position) != MacroTag.Floor)
                return false;

            // Check if there's no other decoration nearby (at least 2 cells away)
            for (int x = -2; x <= 2; x++) {
                for (int y = -2; y <= 2; y++) {
                    Vector2Int checkPos = position + new Vector2Int(x, y);
                    if (dungeonTags.GetMicroTag(checkPos) != MicroTag.None)
                        return false;
                }
            }

            return true;
        }

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) {
            if (Context.Matrix[position] != BlockType.EmptySpace ||
                dungeonTags.GetMacroTag(position) != MacroTag.Floor)
                return false;

            // Check if there's no other decoration nearby (at least 2 cells away)
            for (int x = -2; x <= 2; x++) {
                for (int y = -2; y <= 2; y++) {
                    Vector2Int checkPos = position + new Vector2Int(x, y);
                    if (dungeonTags.GetMicroTag(checkPos) != MicroTag.None)
                        return false;
                }
            }

            return true;
        }
    }

    public class BigFloorDecorationMicroTagRule : IMicroTagRule {
        public int GlobalMaxCount => int.MaxValue;
        public int LocalMaxCount => 3;
        public Dungeon Context { get; set; }
        public MicroTag MicroTag => MicroTag.BigFloorDecoration;

        public GlobalPlaceTagFilter GlobalPlaceTagFilter => new GlobalPlaceTagFilter(GlobalPlaceTag.DefaultRoom);

        public MacroTagFilter MacroTagFilter => new MacroTagFilter(MacroTag.Floor);

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) {
            // Check if we have enough space for a 2x2 decoration
            if (position.x + 1 >= room.RoomBounds.xMax || position.y + 1 >= room.RoomBounds.yMax)
                return false;

            // Check if all 4 cells are empty spaces
            for (int x = 0; x < 2; x++) {
                for (int y = 0; y < 2; y++) {
                    Vector2Int checkPos = position + new Vector2Int(x, y);
                    if (Context.Matrix[checkPos] != BlockType.EmptySpace)
                        return false;
                }
            }

            // Check if the 2 lower blocks have the floor tag
            if (dungeonTags.GetMacroTag(position) != MacroTag.Floor ||
                dungeonTags.GetMacroTag(position + Vector2Int.right) != MacroTag.Floor)
                return false;

            // Check if there's no other decoration nearby (at least 3 cells away)
            for (int x = -3; x <= 3; x++) {
                for (int y = -3; y <= 3; y++) {
                    Vector2Int checkPos = position + new Vector2Int(x, y);
                    if (dungeonTags.GetMicroTag(checkPos) != MicroTag.None)
                        return false;
                }
            }

            return true;
        }

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) =>
            false;
    }

    public class CeilingDecorationMicroTagRule : IMicroTagRule {
        public int GlobalMaxCount => int.MaxValue;
        public int LocalMaxCount => 60;
        public Dungeon Context { get; set; }
        public MicroTag MicroTag => MicroTag.CeilingDecoration;

        public GlobalPlaceTagFilter GlobalPlaceTagFilter => new ();

        public MacroTagFilter MacroTagFilter => new();

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) {
            // Check if there's a wall above
            Vector2Int abovePos = position + Vector2Int.up;
            if (abovePos.y < room.RoomBounds.yMax && Context.Matrix[abovePos] != BlockType.Wall)
                return false;

            // Check if the cell is empty
            if (Context.Matrix[position] != BlockType.EmptySpace)
                return false;

            // Check if there's no other decoration nearby (at least 2 cells away)
            for (int x = -2; x <= 2; x++) {
                for (int y = -2; y <= 2; y++) {
                    Vector2Int checkPos = position + new Vector2Int(x, y);
                    if (checkPos.x < room.RoomBounds.xMin || checkPos.x >= room.RoomBounds.xMax ||
                        checkPos.y < room.RoomBounds.yMin || checkPos.y >= room.RoomBounds.yMax)
                        continue;
                    if (dungeonTags.GetMicroTag(checkPos) == MicroTag.CeilingDecoration)
                        return false;
                }
            }

            return true;
        }

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) {
            Vector2Int abovePos = position + Vector2Int.up;
            
            if (Context.Matrix[abovePos] != BlockType.Wall)
                return false;

            // Check if the cell is empty
            if (Context.Matrix[position] != BlockType.EmptySpace)
                return false;

            // Check if there's no other decoration nearby (at least 2 cells away)
            for (int x = -2; x <= 2; x++) {
                for (int y = -2; y <= 2; y++) {
                    Vector2Int checkPos = position + new Vector2Int(x, y);
                    if (!Context.Matrix.IsPositionInBounds(checkPos))
                        continue;
                    if (dungeonTags.GetMicroTag(checkPos) == MicroTag.CeilingDecoration)
                        return false;
                }
            }

            return true;
        }
    }

    public class WallDecorationMicroTagRule : IMicroTagRule {
        public int GlobalMaxCount => int.MaxValue;
        public int LocalMaxCount => 4;
        public Dungeon Context { get; set; }
        public MicroTag MicroTag => MicroTag.WallDecoration;

        public GlobalPlaceTagFilter GlobalPlaceTagFilter => new GlobalPlaceTagFilter(GlobalPlaceTag.DefaultRoom);

        public MacroTagFilter MacroTagFilter => new MacroTagFilter(MacroTag.Floor);

        public bool IsValidForPosition(Vector2Int position, Room room, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) {
            // Check if there's a wall on either side
            Vector2Int leftPos = position + Vector2Int.left;
            Vector2Int rightPos = position + Vector2Int.right;
            bool hasWallOnLeft = leftPos.x >= room.RoomBounds.xMin && Context.Matrix[leftPos] == BlockType.Wall;
            bool hasWallOnRight = rightPos.x < room.RoomBounds.xMax && Context.Matrix[rightPos] == BlockType.Wall;
            
            if (!hasWallOnLeft && !hasWallOnRight)
                return false;

            // Check if the cell is empty
            if (Context.Matrix[position] != BlockType.EmptySpace)
                return false;

            // Check if there's no other decoration nearby (at least 2 cells away)
            for (int x = -2; x <= 2; x++) {
                for (int y = -2; y <= 2; y++) {
                    Vector2Int checkPos = position + new Vector2Int(x, y);
                    if (checkPos.x < room.RoomBounds.xMin || checkPos.x >= room.RoomBounds.xMax ||
                        checkPos.y < room.RoomBounds.yMin || checkPos.y >= room.RoomBounds.yMax)
                        continue;
                    if (dungeonTags.GetMicroTag(checkPos) != MicroTag.None)
                        return false;
                }
            }

            return true;
        }

        public bool IsValidForPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, MacroTag macroTag,
                                       DungeonTags dungeonTags) =>
            false;
    }
}