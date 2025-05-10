using Zenject;

namespace _Project.Infrastructure.MVP.Core {
    public class WindowServiceInstaller : Installer<WindowServiceInstaller> {
        public override void InstallBindings() {
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
            Container.Bind<IPresenterFactory>().To<PresenterFactory>().AsSingle();
            Container.Bind<ActiveWindowDataHolder>().AsSingle();
        }
    }
}