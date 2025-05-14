using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public class DropItemOnDeath : MonoBehaviour {
        [SerializeField] private EntityHealth _entityHealth;
        [SerializeField] private GameObject _itemPrefab;

        private void OnEnable() =>
            _entityHealth.OnDeath += DropItem;

        private void OnDisable() =>
            _entityHealth.OnDeath -= DropItem;

        private void DropItem() {
            gameObject.SetActive(false);
            Instantiate(_itemPrefab, transform.position, Quaternion.identity);
        }
    }
}