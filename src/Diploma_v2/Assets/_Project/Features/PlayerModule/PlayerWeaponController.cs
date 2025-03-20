using System;
using _Project.Features.InputModule;
using _Project.Features.WeaponModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.PlayerModule
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [Inject] private IInputService _inputService;
        [Inject] private EntityWeaponDataHolder _weaponDataHolder;

        private void OnEnable() =>
            _inputService.OnAttackStarted += FireWeapons;

        private void Update() =>
            RotateWeaponsInLookDirection();

        private void OnDisable() =>
            _inputService.OnAttackStarted -= FireWeapons;

        private void FireWeapons()
        {
            foreach (IWeapon weapon in _weaponDataHolder.EquippedWeapons)
                if (weapon is IShootable shootable)
                    shootable.Shoot();
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