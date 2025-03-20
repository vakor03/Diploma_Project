using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponModule
{
    public class SelfRegisterWeapon : MonoBehaviour
    {
        [Inject] private EntityWeaponDataHolder _dataHolder;
        private IWeapon _associatedWeapon;

        private void Awake() =>
            _associatedWeapon = GetComponent<IWeapon>();

        private void OnEnable() =>
            _dataHolder.AddWeapon(_associatedWeapon);

        private void OnDisable() =>
            _dataHolder.RemoveWeapon(_associatedWeapon);
    }
}