using System;
using UnityEngine.Tilemaps;

namespace _Project.Features.LevelGeneratorModule.TilemapsBootstrap {
    [Serializable]
    public class TilemapConfiguration {
        public Tilemap Prefab;
        public int OrderInLayer;
    }
}