using _Project.Features.MapGeneration.BSP;
using Zenject;

namespace _Project.Features.MapGeneration {
    public class DungeonGeneratorInstaller : Installer<DungeonGeneratorInstaller> {
        public override void InstallBindings() {
            BindMacroLayoutGenerationService();
            BindGenerationService();
        }

        private void BindGenerationService() =>
            Container.Bind<IDungeonGeneratorService>().To<DungeonGeneratorService>().AsSingle();

        private void BindMacroLayoutGenerationService() =>
            Container.Bind<IMacroLayoutDungeonGenerationService>().To<BSPDungeonGenerator>().AsSingle();
    }
}