using Zenject;

namespace _Project.Features.ExperienceModule.Orbs {
    public class EnumHelpersInstaller : Installer<EnumHelpersInstaller> {
        public override void InstallBindings() =>
            Container.Bind<IEnumValuesProvider>().To<EnumValuesProvider>().AsSingle();
    }
}