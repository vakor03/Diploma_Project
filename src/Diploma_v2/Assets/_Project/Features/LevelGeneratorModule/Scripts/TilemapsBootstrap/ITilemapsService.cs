using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Features.LevelGeneratorModule.TilemapsBootstrap {
    public interface ITilemapsService {
        public void SetTile(TilemapType tilemapType, Vector2Int position, TileBase tile);
        public void SetTile(TilemapType tilemapType, Vector3Int position, TileBase tile);
        public void SetTiles(TilemapType tilemapType, Vector2Int[] positions, TileBase[] tiles);
        public void SetTiles(TilemapType tilemapType, Vector3Int[] positions, TileBase[] tiles);
    }
}