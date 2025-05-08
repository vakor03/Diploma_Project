using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.LevelGeneratorModule.TilemapsBootstrap {
    [CreateAssetMenu(fileName = nameof(TilemapsBootstrapConfiguration) + "_Default",
        menuName = "Configurations/LevelGenerationModule/" + nameof(TilemapsBootstrapConfiguration))]
    public class TilemapsBootstrapConfiguration : ScriptableObject {
        [field: SerializeField] public Grid GridPrefab { get; private set; }
        [field: SerializeField] public SerializedDictionary<TilemapType, TilemapConfiguration> Tilemaps { get; private set; }
    }
}