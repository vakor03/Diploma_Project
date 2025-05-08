using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Features.LevelGeneratorModule.TilemapsBootstrap {
    public class TilemapsService : ITilemapsService {
        private readonly TilemapsDataHolder _tilemapsDataHolder;

        public TilemapsService(TilemapsDataHolder tilemapsDataHolder) =>
            _tilemapsDataHolder = tilemapsDataHolder;

        public void SetTile(TilemapType tilemapType, Vector2Int position, TileBase tile) {
            Vector3Int vector3Int = new Vector3Int(position.x, position.y, 0);
            SetTile(tilemapType, vector3Int, tile);
        }

        public void SetTile(TilemapType tilemapType, Vector3Int position, TileBase tile) {
            if (_tilemapsDataHolder.TryGetTilemap(tilemapType, out Tilemap tilemap))
                tilemap.SetTile(position, tile);
            else
                Debug.LogError($"Tilemap {tilemapType.ToString()} not found");
        }

        public void SetTiles(TilemapType tilemapType, Vector2Int[] positions, TileBase[] tiles) {
            Vector3Int[] vector3Ints = positions.Select(position => new Vector3Int(position.x, position.y, 0))
                .ToArray();

            SetTiles(tilemapType, vector3Ints, tiles);
        }

        public void SetTiles(TilemapType tilemapType, Vector3Int[] positions, TileBase[] tiles) {
            if (_tilemapsDataHolder.TryGetTilemap(tilemapType, out Tilemap tilemap))
                tilemap.SetTiles(positions, tiles);
            else
                Debug.LogError($"Tilemap {tilemapType.ToString()} not found");
        }
    }
}