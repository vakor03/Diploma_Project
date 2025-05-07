using Zenject;

namespace _Project.Features.MapGeneration.CA {
    public class CaveRoomCarveInstaller : Installer<CaveRoomCarveInstaller> {
        public override void InstallBindings() {
            Container.Bind<ICaveRoomCarveService>().To<CellularAutomataCaveGeneratorService>().AsSingle();
        }
    }
}