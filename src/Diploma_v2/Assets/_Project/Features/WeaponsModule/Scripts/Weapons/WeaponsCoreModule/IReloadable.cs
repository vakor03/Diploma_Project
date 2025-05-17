namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule {
	public interface IReloadable {
		public bool IsReloading { get; }
		public void StartReloading();
	}
}