using _Project.Features.StatsModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponModule
{
    public class SelfRegisterWeapon : MonoBehaviour
    {
        [Inject] private EntityWeaponDataHolder _dataHolder;
        [SerializeField] private DefaultEnemyStats _defaultEnemyStats;
        private IWeapon _associatedWeapon;

        private void Awake() =>
            _associatedWeapon = GetComponent<IWeapon>();

        private void OnEnable() =>
            _dataHolder.AddWeapon(_associatedWeapon, new StatService<WeaponStats>(new StatDataHolder<WeaponStats>()));

        private void OnDisable() =>
            _dataHolder.RemoveWeapon(_associatedWeapon);
    }
}