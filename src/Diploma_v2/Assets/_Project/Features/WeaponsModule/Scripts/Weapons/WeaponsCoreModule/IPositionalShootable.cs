using _Project.Features.WeaponModule;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule {
	public interface IPositionalShootable : IShootable {
		public void Shoot(Vector2 position);
	}
}