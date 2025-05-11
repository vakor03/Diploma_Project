using Zenject;

namespace _Project.Features.UpgradesModule {
    public class PlayerWeaponsInstaller : Installer<PlayerWeaponsInstaller> {
        public override void InstallBindings() =>
            Container.Bind<IPlayerWeaponService>().To<PlayerWeaponService>().AsSingle();
    }
}