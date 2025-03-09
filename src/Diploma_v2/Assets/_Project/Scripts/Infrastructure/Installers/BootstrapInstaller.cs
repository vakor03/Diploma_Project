using _Project.Scripts.Core.HUB;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers {
    public class BootstrapInstaller : MonoInstaller {
        public override void InstallBindings() {
            Container.BindInterfacesTo<HubCharacterVariantFactory>()
                .AsSingle();

            PopulateAllCharactersToChooseModelSystemInstaller.Install(Container);
        }
    }
}