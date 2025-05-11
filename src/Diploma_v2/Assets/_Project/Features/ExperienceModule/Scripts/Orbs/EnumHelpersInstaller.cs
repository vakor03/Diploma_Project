using Zenject;

namespace _Project.Features.ExperienceModule {
    public class EnumHelpersInstaller : Installer<EnumHelpersInstaller> {
        public override void InstallBindings() =>
            Container.Bind<IEnumValuesProvider>().To<EnumValuesProvider>().AsSingle();
    }
}