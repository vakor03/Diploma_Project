using System;
using UnityEngine;

namespace _Project.Features.UpgradesModule {
    [Serializable]
    public class WeaponUnlockData
    {
        public GameObject weaponPrefab;
        public string weaponId;
        public Vector3 spawnOffset = Vector3.zero;
        public bool isPassiveWeapon = false;
    }
}