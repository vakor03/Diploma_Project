using System.Collections.Generic;
using _Project.Features.MapGeneration.BSP;
using UnityEngine;

namespace _Project.Features.MapGeneration.RoomConnections {
    public interface IRoomConnectorService
    {
        List<(Vector2Int start, Vector2Int end)> GetConnections(List<Room> rooms);
    }
}