using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Features.LevelGeneratorModule.TilemapsBootstrap {
    public class TilemapsDataHolder {
        private Dictionary<TilemapType, Tilemap> _tilemaps = new();
        public IReadOnlyDictionary<TilemapType, Tilemap> Tilemaps => _tilemaps;

        public void RegisterTilemap(TilemapType tilemapType, Tilemap tilemap) {
            if (_tilemaps.ContainsKey(tilemapType)) {
                Debug.LogError($"Tilemap of type {tilemapType} already registered.");
                return;
            }

            _tilemaps[tilemapType] = tilemap;
        }

        public void UnregisterTilemap(TilemapType tilemapType) {
            if (!_tilemaps.ContainsKey(tilemapType)) {
                Debug.LogError($"Tilemap of type {tilemapType} not found.");
                return;
            }

            _tilemaps.Remove(tilemapType);
        }

        public bool TryGetTilemap(TilemapType tilemapType, out Tilemap tilemap) =>
            _tilemaps.TryGetValue(tilemapType, out tilemap);
    }
}