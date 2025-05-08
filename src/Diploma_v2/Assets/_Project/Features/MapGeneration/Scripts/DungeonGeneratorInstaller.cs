using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.CA;
using _Project.Features.MapGeneration.Drukard;
using _Project.Features.MapGeneration.RoomConnections;
using _Project.Features.MapGeneration.Tagging;
using Zenject;

namespace _Project.Features.MapGeneration {
    public class DungeonGeneratorInstaller : Installer<DungeonGeneratorInstaller> {
        public override void InstallBindings() {
            BindMacroLayoutGenerationService();
            BindGenerationService();
            BindCaveCarvingService();
            BindRoomConnectorService();
            BindCorridorGeneratorService();
            BindDungeonTagService();
        }

        private void BindDungeonTagService() =>
            Container.Bind<IDungeonTagService>().To<DungeonTagService>().AsSingle();

        private void BindCorridorGeneratorService() =>
            Container.Bind<ICorridorGeneratorService>().To<DrunkardCorridorGeneratorService>().AsSingle();

        private void BindRoomConnectorService() =>
            Container.Bind<IRoomConnectorService>().To<ClosestRoomConnectorService>().AsSingle();

        private void BindCaveCarvingService() =>
            Container.Bind<ICaveRoomCarveService>().To<CellularAutomataCaveGeneratorService>().AsSingle();

        private void BindGenerationService() =>
            Container.Bind<IDungeonGeneratorService>().To<DungeonGeneratorService>().AsSingle();

        private void BindMacroLayoutGenerationService() =>
            Container.Bind<IMacroLayoutDungeonGenerationService>().To<BSPDungeonGenerator>().AsSingle();
    }
}