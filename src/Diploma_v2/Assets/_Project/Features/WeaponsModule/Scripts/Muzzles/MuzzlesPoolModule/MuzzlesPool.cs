using _Project.Features.ObjectPoolModule;

namespace _Project.Features.WeaponsModule.Scripts.Muzzles.MuzzlesPoolModule {
	public class MuzzlesPool : GenericObjectPool<MonoPooledMuzzle, MuzzleType> {
		protected MuzzlesPool(IGenericPoolFactory<MonoPooledMuzzle, MuzzleType> genericPoolFactory,
		                      IObjectPoolConfiguration<MuzzleType> objectPoolConfiguration) : base(genericPoolFactory,
			objectPoolConfiguration) { }
	}
}