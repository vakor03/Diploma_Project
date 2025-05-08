using System;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;
using UnityEngine.Tilemaps;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace _Project.Features.LevelGeneratorModule.TilemapsBootstrap {
    [Serializable]
    public class TilemapConfiguration {
        public Tilemap Prefab;
        public int OrderInLayer;
        public Layer Layer;
    }
}