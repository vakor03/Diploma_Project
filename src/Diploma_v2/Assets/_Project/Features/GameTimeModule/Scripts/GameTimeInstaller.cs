using Zenject;

namespace _Project.Features.GameTimeModule {
    public class GameTimeInstaller : Installer<GameTimeInstaller> {
        public override void InstallBindings() {
            Container.Bind<IGamePauseService>().To<GamePauseService>().AsSingle();
        }
    }
}