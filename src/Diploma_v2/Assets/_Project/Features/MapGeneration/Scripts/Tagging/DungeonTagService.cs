using System.Collections.Generic;
using _Project.Extensions.EnumerableExtensions;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.SeedModule;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging {
    public interface IDungeonTagService {
        public DungeonTags TagAllRegions(Dungeon dungeon, List<IRoomTagRule> roomTagRules, List<IRoomSubSpaceTagRule> roomSubSpaceTagRules);
    }

    public class DungeonTagService : IDungeonTagService {
        private List<IRoomTagRule> _roomTagRules;
        private readonly ISeedService _seedService;
        public DungeonTagService(ISeedService seedService) {
            _seedService = seedService;
        }

        public DungeonTags TagAllRegions(Dungeon dungeon, List<IRoomTagRule> roomTagRules, List<IRoomSubSpaceTagRule> roomSubSpaceTagRules) {
            DungeonTags dungeonTags = new();
            foreach (IRoomTagRule roomTagRule in roomTagRules)
                roomTagRule.Context = dungeon;
            foreach (IRoomSubSpaceTagRule roomSubSpaceTagRule in roomSubSpaceTagRules)
                roomSubSpaceTagRule.Context = dungeon;

            foreach (IRoomTagRule roomTagRule in roomTagRules.InRandomOrder(_seedService.GetRandom()))
                ApplyTagRule(roomTagRule, dungeon.Rooms, dungeonTags);
            
            foreach (Room dungeonRoom in dungeon.Rooms.InRandomOrder(_seedService.GetRandom()))
            foreach (IRoomSubSpaceTagRule roomSubSpaceTagRule in roomSubSpaceTagRules.InRandomOrder(_seedService.GetRandom()))
                ApplySubSpaceTagRule(roomSubSpaceTagRule, dungeonRoom, dungeonTags);

            return dungeonTags;
        }
        
        private void ApplyTagRule(IRoomTagRule roomTagRule, List<Room> rooms, DungeonTags dungeonTags) {
            int appliedCount = 0;
            foreach (Room room in rooms) {
                if (roomTagRule.IsValid(room)) {
                    dungeonTags.AddRoomTag(roomTagRule.RoomTag, room);
                    appliedCount++;
                }

                if (appliedCount >= roomTagRule.MaxCount) {
                    break;
                }
            }
        }
        
        private void ApplySubSpaceTagRule(IRoomSubSpaceTagRule roomSubSpaceTagRule, Room room, DungeonTags dungeonTags) {
            int appliedCount = 0;
                foreach (Vector2Int position in room.Cells) {
                    if (roomSubSpaceTagRule.IsValid(position, room, dungeonTags.GetTagForRoom(room))) {
                        dungeonTags.AddSubSpaceTag(roomSubSpaceTagRule.SubSpaceTag, position);
                        appliedCount++;
                    }

                    if (appliedCount >= roomSubSpaceTagRule.MaxCount)
                        break;
            }
        }
    }

    public enum SubSpaceTag {
        None = 0,
        PlayerSpawnPoint = 1,
    }

    public interface IRoomTagRule {
        public int MaxCount { get; }
        public Dungeon Context { set; }
        public RoomTag RoomTag { get; }
        public bool IsValid(Room room);
    }
    
    public interface IRoomSubSpaceTagRule {
        public int MaxCount { get; }
        public Dungeon Context { set; }
        public SubSpaceTag SubSpaceTag { get; }
        public bool IsValid(Vector2Int position, Room room, RoomTag roomTag);
    }
    
    public class InitialRoomTagRule : IRoomTagRule {
        public int MaxCount => 1;
        public Dungeon Context { get; set; }
        public RoomTag RoomTag => RoomTag.InitialRoom;

        public bool IsValid(Room room) =>
            room.Cells.Count > 0;
    }

    public class PlayerPositionSubSpaceTagRule : IRoomSubSpaceTagRule {
        public int MaxCount => 1;
        public Dungeon Context { get; set; }
        public SubSpaceTag SubSpaceTag => SubSpaceTag.PlayerSpawnPoint;

        public bool IsValid(Vector2Int position, Room room, RoomTag roomTag) {
            if (roomTag != RoomTag.InitialRoom)
                return false;

            if (room.Cells.Contains(position + Vector2Int.down))
                return false;

            return true;
        }
    }
}