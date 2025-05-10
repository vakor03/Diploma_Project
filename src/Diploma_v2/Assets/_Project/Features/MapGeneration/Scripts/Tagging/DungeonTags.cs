using System.Collections.Generic;
using System.Linq;
using _Project.Features.MapGeneration.BSP;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging {
    public class DungeonTags
    {
        private Dictionary<Room, GlobalPlaceTag> _roomGlobalPlaceTags = new();
        private Dictionary<Tunnel, GlobalPlaceTag> _tunnelGlobalPlaceTags = new();
        private Dictionary<Vector2Int, MacroTag> _positionMacroTags = new();
        private Dictionary<Vector2Int, MicroTag> _positionMicroTags = new();

        public void AddGlobalPlaceTag(GlobalPlaceTag tag, Room room)
        {
            _roomGlobalPlaceTags[room] = tag;
        }
    
        public void AddGlobalPlaceTag(GlobalPlaceTag tag, Tunnel tunnel)
        {
            _tunnelGlobalPlaceTags[tunnel] = tag;
        }

        public void AddMacroTag(MacroTag tag, Vector2Int position)
        {
            _positionMacroTags[position] = tag;
        }

        public void AddMicroTag(MicroTag tag, Vector2Int position)
        {
            _positionMicroTags[position] = tag;
        }

        public GlobalPlaceTag GetGlobalPlaceTagForRoom(Room room)
        {
            return _roomGlobalPlaceTags.GetValueOrDefault(room, GlobalPlaceTag.None);
        }
    
        public GlobalPlaceTag GetGlobalPlaceTagForTunnel(Tunnel tunnel)
        {
            return _tunnelGlobalPlaceTags.GetValueOrDefault(tunnel, GlobalPlaceTag.None);
        }

        public MacroTag GetMacroTag(Vector2Int position)
        {
            return _positionMacroTags.GetValueOrDefault(position, MacroTag.None);
        }

        public MicroTag GetMicroTag(Vector2Int position)
        {
            return _positionMicroTags.GetValueOrDefault(position, MicroTag.None);
        }

        // Helper methods for queries
        public List<Room> GetRoomsWithTag(GlobalPlaceTag tag)
        {
            return _roomGlobalPlaceTags.Where(pair => pair.Value == tag).Select(pair => pair.Key).ToList();
        }
    
        public List<Vector2Int> GetPositionsWithMacroTag(MacroTag tag)
        {
            return _positionMacroTags.Where(pair => pair.Value == tag).Select(pair => pair.Key).ToList();
        }
    
        public List<Vector2Int> GetPositionsWithMicroTag(MicroTag tag)
        {
            return _positionMicroTags.Where(pair => pair.Value == tag).Select(pair => pair.Key).ToList();
        }
    
        public Dictionary<Vector2Int, MacroTag> GetAllMacroTaggedPositions()
        {
            return new Dictionary<Vector2Int, MacroTag>(_positionMacroTags);
        }
    
        public Dictionary<Vector2Int, MicroTag> GetAllMicroTaggedPositions()
        {
            return new Dictionary<Vector2Int, MicroTag>(_positionMicroTags);
        }
    
        public IReadOnlyDictionary<Vector2Int, MacroTag> MacroTaggedPositions => _positionMacroTags;
        public IReadOnlyDictionary<Vector2Int, MicroTag> MicroTaggedPositions => _positionMicroTags;

    }
}