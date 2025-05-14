using System;
using _Project.Features.InputModule;
using _Project.Features.WeaponModule;
using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;
using Zenject;
using IShootable = _Project.Features.WeaponModule.IShootable;

namespace _Project.Features.PlayerModule
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [Inject] private IInputService _inputService;
        [Inject] private EntityWeaponDataHolder _weaponDataHolder;

        private void OnEnable() =>
            _inputService.OnAttackStarted += FireWeapons;

        private void Update() {
            RotateWeaponsInLookDirection();
            if (_inputService.IsAttacking) {
                FireWeapons();
            }
        }

        private void OnDisable() =>
            _inputService.OnAttackStarted -= FireWeapons;

        private void FireWeapons()
        {
            foreach (IWeapon weapon in _weaponDataHolder.EquippedWeapons) {
                if (weapon is IShootable shootable)
                    shootable.Shoot();

                if (weapon is IDirectionalShootable dir) {
                    dir.Shoot(_inputService.GetLookDirection());
                }
            }
        }

        private void RotateWeaponsInLookDirection()
        {
            Vector2 lookDirection = _inputService.GetLookDirection();
            if (lookDirection == Vector2.zero)
            {
                return;
            }

            foreach (IWeapon weapon in _weaponDataHolder.EquippedWeapons)
                if (weapon is IRotatable rotatable)
                    rotatable.RotateInDirection(lookDirection);
        }
    }
}