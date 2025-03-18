using _Project.Features.SeedModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.Installers
{
    public class SeedServiceInstaller : Installer<SeedServiceInstaller>
    {
        public override void InstallBindings() =>
            Container.Bind<ISeedService>().To<PredefinedSeedService>().AsSingle();
    }
}