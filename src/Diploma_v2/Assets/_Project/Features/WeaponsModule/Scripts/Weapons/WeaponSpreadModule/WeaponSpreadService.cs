using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponSpreadModule {
	public class WeaponSpreadService : IWeaponSpreadService {
		public Vector2 ApplySpread(Vector2 direction, float spread) {
			float vertical = Random.Range(-spread, spread);
			return (direction + new Vector2(0, vertical)).normalized;
		}
	}
}