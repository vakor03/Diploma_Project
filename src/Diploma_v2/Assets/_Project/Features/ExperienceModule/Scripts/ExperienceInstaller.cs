using _Project.Extensions.ZenjectExtensions;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.ExperienceModule {
    public class ExperienceInstaller : Installer<ExperienceInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<XPLevelConfiguration>(AssetPath.Configuration.XP_LEVEL_CONFIGURATION)
                .AsSingle();
            Container.Bind<IExperienceService>().To<ExperienceService>().AsSingle();
            Container.BindInterfacesTo<SetInitialXPSystem>()
                .AsSingle();
        }
    }
}