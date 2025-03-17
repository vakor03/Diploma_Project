using Zenject;

namespace _Project.Scripts.Infrastructure.Installers {
    public class DataInstaller : Installer<DataInstaller> {
        public override void InstallBindings() {
        }
        
        private void InstallModel<T> () where T : IModel =>
            Container.Bind<T>().AsSingle();
    }
}