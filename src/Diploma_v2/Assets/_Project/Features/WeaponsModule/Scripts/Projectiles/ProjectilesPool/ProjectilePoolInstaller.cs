using _Project.Extensions.ZenjectExtensions;
using _Project.Features.ObjectPoolModule;
using _Project.Scripts.Infrastructure.AssetProviders;
using Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool {
	public class ProjectilePoolInstaller : Installer<ProjectilePoolInstaller> {
		public override void InstallBindings() {
			Container.BindInterfacesAndSelfToFromAddressables<ProjectilePoolConfiguration>(AssetPath.Configuration.PROJECTILE_POOL_CONFIGURATION)
			         .AsSingle();
			
			Container.BindInterfacesAndSelfTo<GenericPooledObjectFactory<MonoPooledProjectile,ProjectileType>>()
			         .AsSingle();

			Container.BindInterfacesAndSelfTo<GenericPoolFactory<MonoPooledProjectile,ProjectileType>>()
			         .AsSingle();

			Container.BindInterfacesAndSelfTo<ProjectilesObjectPool>()
			         .AsSingle();
		}
	}
}