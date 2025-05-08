using System.Collections.Generic;
using _Project.Features.MapGeneration.BSP;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging {
    public class DungeonTags {
        public Dictionary<RoomTag, List<Room>> TaggedRooms = new();
        public Dictionary<SubSpaceTag, List<Vector2Int>> TaggedSubSpaces = new();
        
        public void AddRoomTag(RoomTag roomTag, Room room) {
            if (!TaggedRooms.ContainsKey(roomTag))
                TaggedRooms[roomTag] = new();
            TaggedRooms[roomTag].Add(room);
        }
        
        public void AddSubSpaceTag(SubSpaceTag subSpaceTag, Vector2Int position) {
            if (!TaggedSubSpaces.ContainsKey(subSpaceTag))
                TaggedSubSpaces[subSpaceTag] = new();
            TaggedSubSpaces[subSpaceTag].Add(position);
        }

        public RoomTag GetTagForRoom(Room room) {
            foreach (KeyValuePair<RoomTag, List<Room>> roomTag in TaggedRooms)
                if (roomTag.Value.Contains(room))
                    return roomTag.Key;

            return RoomTag.None;
        }
    }
}