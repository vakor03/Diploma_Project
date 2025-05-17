using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule {
	public enum WeaponType {
		[Tooltip("Projectile AK47")]
		AK47 = 0,

		[Tooltip("Raycast AK47")]
		AK47_Raycast = 1,
		Shotgun = 2,
		Sniper = 3,
		Minigun = 4,
	}
}