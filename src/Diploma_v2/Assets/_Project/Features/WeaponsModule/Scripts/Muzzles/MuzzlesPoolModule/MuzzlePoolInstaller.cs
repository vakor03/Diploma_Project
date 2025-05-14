using _Project.Extensions.ZenjectExtensions;
using _Project.Features.ObjectPoolModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Muzzles.MuzzlesPoolModule {
	public class MuzzlePoolInstaller : Installer<MuzzlePoolInstaller> {
		public override void InstallBindings() {
			Container.BindInterfacesAndSelfToFromAddressables<MuzzlesPoolConfiguration>(AssetPath.Configuration.MUZZLES_POOL_CONFIGURATION)
			         .AsSingle();

			Container.BindInterfacesAndSelfTo<GenericPooledObjectFactory<MonoPooledMuzzle, MuzzleType>>()
			         .AsSingle();

			Container.BindInterfacesAndSelfTo<GenericPoolFactory<MonoPooledMuzzle, MuzzleType>>()
			         .AsSingle();

			Container.BindInterfacesAndSelfTo<MuzzlesPool>()
			         .AsSingle();
		}
	}
}