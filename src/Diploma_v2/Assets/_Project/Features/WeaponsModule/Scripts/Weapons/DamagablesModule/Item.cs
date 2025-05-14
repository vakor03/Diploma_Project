using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public abstract class Item : ScriptableObject {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;

        public abstract void PerformPickupOperation(GameObject picker);

        public Sprite GetSprite() =>
            _icon;
    }
}