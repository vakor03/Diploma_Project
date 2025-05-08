using _Project.Features.LevelGeneratorModule.TilemapsBootstrap;
using _Project.Features.MapGeneration;
using UnityEngine;

namespace _Project.Features.LevelGeneratorModule {
    [CreateAssetMenu(fileName = nameof(LevelConfiguration) + "_Default",
        menuName = "Configurations/LevelGenerationModule/" + nameof(LevelConfiguration))]
    public class LevelConfiguration : ScriptableObject {
        [field: SerializeField] public DungeonGenerationConfiguration DungeonGenerationConfiguration { get; private set; }
        [field: SerializeField] public GameObject TilePrefab { get; private set; }
        [field: SerializeField] public TilemapsBootstrapConfiguration TilemapsBootstrapConfiguration { get; private set; }
        [field: SerializeField] public TilesConfiguration TilesConfiguration { get; private set; }
    }
}