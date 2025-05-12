using System.Collections.Generic;
using _Project.Features.StatsModule;
using UnityEngine;

namespace _Project.Features.WeaponModule
{
    public class EntityWeaponDataHolder
    {
        public IReadOnlyList<IWeapon> EquippedWeapons => _equippedWeapons;

        private readonly List<IWeapon> _equippedWeapons = new List<IWeapon>();

        public void ClearAllWeapons() =>
            _equippedWeapons.Clear();

        public void AddWeapon(IWeapon weapon, IStatService<WeaponStats> weaponStats)
        {
            if (_equippedWeapons.Contains(weapon))
            {
                Debug.LogError("Weapon is already equipped!");
                return;
            }

            _equippedWeapons.Add(weapon);
            weapon.InitWeaponStats(weaponStats);
        }

        public void RemoveWeapon(IWeapon weapon) =>
            _equippedWeapons.Remove(weapon);
    }
}