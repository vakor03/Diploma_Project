using UnityEngine;

namespace _Project.Features.UpgradesModule {
    public interface IPlayerWeaponService
    {
        public void ModifyWeapon(GameObject target, string weaponId, string modifierName, float value, bool isPercentage);
        public void UnlockWeapon(GameObject target, string weaponId, GameObject weaponPrefab, Vector3 offset, bool isPassive);
        public bool IsWeaponUnlocked(string weaponId);
    }
}