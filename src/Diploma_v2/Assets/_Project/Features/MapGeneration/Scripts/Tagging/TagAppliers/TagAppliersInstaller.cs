using Zenject;

namespace _Project.Features.MapGeneration.Tagging.TagAppliers {
    public class TagAppliersInstaller : Installer<TagAppliersInstaller> {
        public override void InstallBindings() {
            Container.Bind<IGlobalPlaceTagApplier>().To<GlobalPlaceTagApplier>().AsSingle();
            Container.Bind<IMacroTagApplier>().To<MacroTagApplier>().AsSingle();
            Container.Bind<IMicroTagApplier>().To<MicroTagApplier>().AsSingle();
        }
    }
}