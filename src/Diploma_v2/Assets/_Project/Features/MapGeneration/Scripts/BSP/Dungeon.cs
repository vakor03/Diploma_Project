using System;
using System.Collections.Generic;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.MapGeneration.Tagging;
using UnityEngine;

namespace _Project.Features.MapGeneration.BSP {
    public class Dungeon {
        public Matrix<int> Matrix;
        public List<Room> Rooms;
        public List<Tunnel> Tunnels;
        public DungeonTags Tags;
    }

    public class Room {
        public RectInt PartitionBounds;
        public RectInt RoomBounds;
        public List<Vector2Int> Cells;
        
        public List<Room> ConnectedRooms; 
    }

    public class Tunnel {
        public Vector2Int Start;
        public Vector2Int End;
        public List<Vector2Int> Cells;
    }

    public enum RoomTag {
        None = 0,
        InitialRoom = 1,
    }
}