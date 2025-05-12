using _Project.Features.StatsModule;
using _Project.Features.UpgradesModule.API;
using UnityEngine;

namespace _Project.Features.UpgradesModule {
    public interface IPlayerWeaponService {
        public void ModifyWeapon(WeaponType weaponType, WeaponStats statToModify, float value, bool isPercentage);
        public void UnlockWeapon(WeaponType weaponType);
        public bool IsWeaponUnlocked(WeaponType weaponType);
    }

    public class PlayerWeaponService : IPlayerWeaponService {
        private readonly PlayerWeaponModel _playerWeaponModel;
        private readonly IStatService<WeaponStats> _statService;

        public PlayerWeaponService(PlayerWeaponModel playerWeaponModel) {
            _playerWeaponModel = playerWeaponModel;
            // _statService = statService;
        }

        public void ModifyWeapon(WeaponType weaponType, WeaponStats statToModify, float value, bool isPercentage) {
            Debug.Assert(IsWeaponUnlocked(weaponType));

            if (isPercentage)
                _statService.ModifyStatPercentage(statToModify, value);
            else
                _statService.ModifyStat(statToModify, value);
        }

        public void UnlockWeapon(WeaponType weaponType) =>
            _playerWeaponModel.UnlockWeapon(weaponType);

        public bool IsWeaponUnlocked(WeaponType weaponType) =>
            _playerWeaponModel.IsWeaponUnlocked(weaponType);
    }
}