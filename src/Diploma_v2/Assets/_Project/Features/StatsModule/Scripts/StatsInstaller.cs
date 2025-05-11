using _Project.Extensions.ZenjectExtensions;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.StatsModule {
    public class StatsInstaller : Installer<StatsInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<DefaultStatsDatabase>(AssetPath.Configuration.DEFAULT_STATS_DATABASE).AsSingle();

            Container.Bind<StatDataHolder<PlayerStats>>().AsSingle();
            Container.Bind<StatDataHolder<WeaponStats>>().AsTransient();
            Container.Bind<StatDataHolder<EnemyStats>>().AsTransient();

            Container.Bind<IStatService<PlayerStats>>().To<StatService<PlayerStats>>().AsSingle();
            Container.Bind<IStatService<WeaponStats>>().To<StatService<WeaponStats>>().AsTransient();
            Container.Bind<IStatService<EnemyStats>>().To<StatService<EnemyStats>>().AsTransient();
        }
    }
}