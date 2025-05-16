using _Project.Extensions.ZenjectExtensions;
using _Project.Features.Enemy;
using _Project.Features.EnemyModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

namespace _Project.Features.EnemyModule {
    [CreateAssetMenu(fileName = nameof(EnemyXPConfiguration) + "_Default",
        menuName = "Configurations/EnemyModule/" + nameof(EnemyXPConfiguration))]
    public class EnemyXPConfiguration : ScriptableObject {
        [field: SerializeField] public SerializedDictionary<EnemyType, float> XPOnDeath { get; private set; }
    }
}

public class EnemyXPInstaller : Installer<EnemyXPInstaller> {
    public override void InstallBindings() {
        Container
            .BindConfigurationFromAddressables<EnemyXPConfiguration>(AssetPath.Configuration.ENEMY_XP_CONFIGURATION)
            .AsSingle();
    }
}