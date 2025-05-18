using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;

namespace _Project.Features.LevelGeneratorModule {
    [CreateAssetMenu(fileName = nameof(TilemapsCollidersConfiguration) + "_Default",
        menuName = "Configurations/LevelGenerationModule/" + nameof(TilemapsCollidersConfiguration))]
    public class TilemapsCollidersConfiguration : ScriptableObject {
        [field: SerializeField] public TilemapColliderConfiguration FloorTilemapCollider { get; private set; }
        [field: SerializeField] public TilemapColliderConfiguration PlatformTilemapCollider { get; private set; }
    }
}