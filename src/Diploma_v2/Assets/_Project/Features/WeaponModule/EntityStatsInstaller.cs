using _Project.Features.StatsModule;
using Zenject;

namespace _Project.Features.WeaponModule {
    public class EntityStatsInstaller : MonoInstaller {
        public override void InstallBindings() {
            Container.Bind<IStatService<EntityStats>>().To<StatService<EntityStats>>().AsSingle();
        }
    }
}