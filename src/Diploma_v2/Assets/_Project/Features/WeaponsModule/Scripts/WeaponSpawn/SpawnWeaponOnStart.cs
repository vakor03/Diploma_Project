using System.Collections.Generic;
using _Project.Features.StatsModule;
using _Project.Features.WeaponModule;
using _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;
using Zenject;

namespace _Project.Features.WeaponsModule.Scripts.WeaponSpawn {
    public class SpawnWeaponOnStart : MonoBehaviour {
        [Inject] private IWeaponSpawnService _weaponSpawnService;
        [Inject] private EntityWeaponDataHolder _entityWeaponDataHolder;
        [Inject] private DefaultStatsDatabase _defaultStatsDatabase;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private Transform _parent;

        private void Start() {
            IWeapon weapon = _weaponSpawnService.SpawnWeapon(_weaponType, _parent, _parent.position);
            _entityWeaponDataHolder.AddWeapon(weapon, WeaponStats());
        }

        private IStatService<WeaponStats> WeaponStats() {
            StatDataHolder<WeaponStats> statDataHolder = new();
            foreach (KeyValuePair<WeaponStats, float> keyValuePair in _defaultStatsDatabase.GetWeaponStats(_weaponType).stats)
                statDataHolder.ModifyStat(keyValuePair.Key, keyValuePair.Value);

            return new StatService<WeaponStats>(statDataHolder);
        }
    }
}