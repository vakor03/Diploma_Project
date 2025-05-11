using Zenject;

namespace _Project.Features.ExperienceModule {
    public class XPOrbsInstaller : Installer<XPOrbsInstaller> {
        public override void InstallBindings() {
            Container.Bind<IXPOrbFactory>().To<XPOrbFactory>().AsSingle();
            Container.Bind<IXPOrbSpawnService>().To<XPOrbSpawnService>().AsSingle();
        }
    }
}