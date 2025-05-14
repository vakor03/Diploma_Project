namespace Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule {
	public interface IProjectile {
		public void Launch();

		public TProjectileBehaviour GetBehaviour<TProjectileBehaviour>()
			where TProjectileBehaviour : IProjectileBehaviour;
	}
}