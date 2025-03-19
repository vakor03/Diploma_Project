using _Project.Features.InputModule;
using Zenject;

namespace _Project.Features.Installers
{
    public class InputSystemInstaller : Installer<InputSystemInstaller>
    {
        public override void InstallBindings() =>
            Container.BindInterfacesTo<InputSystem>().AsSingle();
    }
}