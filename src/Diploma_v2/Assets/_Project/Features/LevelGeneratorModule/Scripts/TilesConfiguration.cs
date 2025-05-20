using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Features.LevelGeneratorModule {
    [CreateAssetMenu(fileName = nameof(TilesConfiguration) + "_Default",
        menuName = "Configurations/LevelGenerationModule/" + nameof(TilesConfiguration))]
    public class TilesConfiguration : ScriptableObject {
        [field: SerializeField] public TileBase FloorTile { get; private set; }
        [field: SerializeField] public TileBase PlatformTile { get; private set; }
        [field: SerializeField] public TileBase FogOfWarTile { get; private set; }
    }
}