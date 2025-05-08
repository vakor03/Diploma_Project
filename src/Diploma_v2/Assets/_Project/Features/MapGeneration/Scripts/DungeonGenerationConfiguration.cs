using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.CA;
using _Project.Features.MapGeneration.Drukard;
using UnityEngine;

namespace _Project.Features.MapGeneration {
    [CreateAssetMenu(fileName = nameof(DungeonGenerationConfiguration) + "_Default",
        menuName = "Configurations/MapGenerationModule/" + nameof(DungeonGenerationConfiguration))]
    public class DungeonGenerationConfiguration : ScriptableObject {
        [field: SerializeField] public Vector2Int DungeonSize { get; private set; }
        [field: SerializeField] public BSPDungeonGeneratorParams BspDungeonGeneratorParams { get; private set; }
        [field: SerializeField] public CAConfig CACaveRoomParams { get; private set; }
        [field: SerializeField] public CorridorConfig CorridorParams { get; private set; }
    }
}