using UnityEngine;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule {
	public interface IDirectionalShootable {
		public void Shoot(Vector2 direction);
	}
}