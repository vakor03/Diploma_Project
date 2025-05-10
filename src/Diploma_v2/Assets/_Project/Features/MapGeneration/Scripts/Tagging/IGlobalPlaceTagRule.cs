using _Project.Features.MapGeneration.BSP;

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

    public class DefaultTunnelGlobalPlaceTagRule : IGlobalPlaceTagRule {
        public int MaxCount => int.MaxValue;
        public Dungeon Context { get; set; }
        public GlobalPlaceTag GlobalPlaceTag => GlobalPlaceTag.MainCorridor;

        public bool IsValidForRoom(Room room) =>
            false;

        public bool IsValidForTunnel(Tunnel tunnel) =>
            true;
        
    }
}