using _Project.Features.MapGeneration.BSP;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging {
    public interface IGlobalPlaceTagRule {
        public int MaxCount { get; }
        public Dungeon Context { set; }
        public GlobalPlaceTag GlobalPlaceTag { get; }
        public bool IsValidForRoom(Room room);
        public bool IsValidForTunnel(Tunnel tunnel);
    }

    public class InitialRoomGlobalPlaceTagRule : IGlobalPlaceTagRule {
        public int MaxCount => 1;
        public Dungeon Context { get; set; }
        public GlobalPlaceTag GlobalPlaceTag => GlobalPlaceTag.InitialRoom;

        public bool IsValidForRoom(Room room) =>
            true;

        public bool IsValidForTunnel(Tunnel tunnel) =>
            false;
    }
    
    public class DefaultRoomGlobalPlaceTagRule : IGlobalPlaceTagRule {
        public int MaxCount => int.MaxValue;
        public Dungeon Context { get; set; }
        public GlobalPlaceTag GlobalPlaceTag => GlobalPlaceTag.DefaultRoom;

        public bool IsValidForRoom(Room room) =>
            true;

        public bool IsValidForTunnel(Tunnel tunnel) =>
            false;
    }

    public class HorizontalTunnelGlobalPlaceTagRule : IGlobalPlaceTagRule {
        public int MaxCount => int.MaxValue;
        public Dungeon Context { get; set; }
        public GlobalPlaceTag GlobalPlaceTag => GlobalPlaceTag.TunnelHorizontal;

        public bool IsValidForRoom(Room room) =>
            false;

        public bool IsValidForTunnel(Tunnel tunnel) =>
            Mathf.Abs(tunnel.Start.x - tunnel.End.x) >= Mathf.Abs(tunnel.Start.y - tunnel.End.y);
    }
    
    public class VerticalTunnelGlobalPlaceTagRule : IGlobalPlaceTagRule {
        public int MaxCount => int.MaxValue;
        public Dungeon Context { get; set; }
        public GlobalPlaceTag GlobalPlaceTag => GlobalPlaceTag.TunnelVertical;

        public bool IsValidForRoom(Room room) =>
            false;

        public bool IsValidForTunnel(Tunnel tunnel) =>
            Mathf.Abs(tunnel.Start.x - tunnel.End.x) < Mathf.Abs(tunnel.Start.y - tunnel.End.y);
    }
    
    
}