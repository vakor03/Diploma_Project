using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponSpreadModule {
	public interface IWeaponSpreadService {
		public Vector2 ApplySpread(Vector2 direction, float spread);
	}
}