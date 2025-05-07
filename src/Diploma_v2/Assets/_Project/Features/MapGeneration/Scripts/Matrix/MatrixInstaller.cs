using _Project.Features.MapGeneration.PNGExproter;
using Zenject;

namespace _Project.Features.MapGeneration.Matrix {
    public class MatrixInstaller : Installer<MatrixInstaller> {
        public override void InstallBindings() =>
            Container.Bind<IMatrixFactory>().To<MatrixFactory>().AsSingle();
    }

    public class DungeonExporterEditorInstaller : Installer<DungeonExporterEditorInstaller> {
        public override void InstallBindings() {
            Container.Bind<DungeonToPNGExporter>().AsSingle();
        }
    }
}