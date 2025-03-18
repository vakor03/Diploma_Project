using _Project.Features.LevelGeneratorModule;
using Zenject;

namespace _Project.Features.Installers
{
    public class LevelGeneratorInstaller : Installer<LevelGeneratorInstaller>
    {
        public override void InstallBindings() =>
            Container.Bind<ILevelGenerationService>().To<LevelGenerationService>().AsSingle();
    }
}