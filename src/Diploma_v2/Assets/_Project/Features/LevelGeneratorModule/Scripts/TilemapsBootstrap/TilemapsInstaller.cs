using Zenject;

namespace _Project.Features.LevelGeneratorModule.TilemapsBootstrap {
    public class TilemapsInstaller : Installer<TilemapsInstaller> {
        public override void InstallBindings() {
            Container.Bind<TilemapsDataHolder>().AsSingle();
            Container.Bind<ITilemapsBootstrapService>().To<TilemapsBootstrapService>().AsSingle();
            Container.Bind<ITilemapsService>().To<TilemapsService>().AsSingle();
        }
    }
}