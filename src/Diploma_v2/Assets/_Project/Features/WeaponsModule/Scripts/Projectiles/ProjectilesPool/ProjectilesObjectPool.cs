using _Project.Features.ObjectPoolModule;
using Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;

namespace _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool {
	public class ProjectilesObjectPool : GenericObjectPool<MonoPooledProjectile, ProjectileType> {
		protected ProjectilesObjectPool(IGenericPoolFactory<MonoPooledProjectile, ProjectileType> genericPoolFactory, IObjectPoolConfiguration<ProjectileType> objectPoolConfiguration) : base(genericPoolFactory, objectPoolConfiguration) { }
	}
}