using Zenject;

namespace _Project.Scripts.Infrastructure.StateMachines.Installers
{
    public class StatesFactoryInstaller : Installer<StatesFactoryInstaller>
    {
        public override void InstallBindings() =>
            Container.Bind<IStatesFactory>().To<StatesFactory>().AsSingle();
    }
}