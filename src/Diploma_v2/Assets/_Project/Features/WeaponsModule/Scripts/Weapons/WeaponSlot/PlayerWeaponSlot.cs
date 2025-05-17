using _Project.Features.InputModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponSlot {
	public class PlayerWeaponSlot : MonoBehaviour {
		private IInputService _inputService;
		private IDirectionalShootable _weapon;

		[Inject]
		public void InjectDependencies(IInputService inputService) =>
			_inputService = inputService;

		private void Awake() =>
			_weapon = GetComponentInChildren<IDirectionalShootable>();

		private void Update() {
			if (_inputService.IsAttacking)
				_weapon.Shoot(transform.right);
		}
	}
}