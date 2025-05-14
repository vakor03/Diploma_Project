using System;
using _Project.Features.WeaponModule;
using UnityEngine;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule {
    public class WeaponSlot : MonoBehaviour {
        public IWeapon Weapon { get; private set; }

        private void Awake() =>
            Weapon = GetComponentInChildren<IWeapon>();
    }
}