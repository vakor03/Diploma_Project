using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure;
using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;

namespace _Project.Features.UpgradesModule.API {
    public class PlayerWeaponModel : IModel {
        private readonly List<WeaponType> _unlockedWeapons  = new();
        
        public IReadOnlyList<WeaponType> UnlockedWeapons => _unlockedWeapons.AsReadOnly();
        
        public event Action<WeaponType> OnWeaponUnlocked;
        
        public void UnlockWeapon(WeaponType weaponType) {
            if (_unlockedWeapons.Contains(weaponType)) return;
            
            _unlockedWeapons.Add(weaponType);
            OnWeaponUnlocked?.Invoke(weaponType);
        }
        
        public bool IsWeaponUnlocked(WeaponType weaponType) =>
            _unlockedWeapons.Contains(weaponType);
    }
}