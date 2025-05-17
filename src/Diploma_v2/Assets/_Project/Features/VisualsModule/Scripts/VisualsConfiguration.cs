using UnityEngine;

namespace _Project.Features.VisualsModule {
    [CreateAssetMenu(fileName = nameof(VisualsConfiguration) + "_Default",
        menuName = "Configurations/VisualsModule/" + nameof(VisualsConfiguration))]
    public class VisualsConfiguration : ScriptableObject {
        [field: SerializeField] public GameObject BackgroundPrefab { get; private set; }
    }
}