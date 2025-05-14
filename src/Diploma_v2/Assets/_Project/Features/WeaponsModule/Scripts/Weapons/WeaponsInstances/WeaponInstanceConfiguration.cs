using Features.WeaponsModule.Scripts.Weapons.WeaponsCoreModule;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
    [CreateAssetMenu(fileName = nameof(WeaponInstanceConfiguration) + "_Default",
        menuName = "Configurations/WeaponsModule/" + nameof(WeaponInstanceConfiguration))]

    public class WeaponInstanceConfiguration : ScriptableObject {
        [field: SerializeField] public SerializedDictionary<WeaponType, GameObject> WeaponPrefabs { get; private set; }
    }
}