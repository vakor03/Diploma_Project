using UnityEngine;

namespace _Project.Features.UIModule.SingleUpgradeUI.Factory {
    [CreateAssetMenu(fileName = nameof(SingleUpgradeFactoryConfiguration) + "_Default",
        menuName = "Configurations/UIModule/" + nameof(SingleUpgradeFactoryConfiguration))]
    public class SingleUpgradeFactoryConfiguration : ScriptableObject {
        [field: SerializeField] public SingleUpgradeViewBase SingleUpgradePrefab { get; private set; }
    }
}