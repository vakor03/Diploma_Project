using _Project.Scripts.Core.HUB;
using _Project.Scripts.Core.Windows;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers {
    public class DataInstaller : Installer<DataInstaller> {
        public override void InstallBindings() {
            InstallModel<NullModel>();
            InstallModel<AllCharactersToChooseModel>();
            InstallModel<SelectedCharacterModel>();
        }
        
        private void InstallModel<T> () where T : IModel =>
            Container.Bind<T>().AsSingle();
    }
}