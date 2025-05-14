using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Weapons.DamagablesModule {
    public abstract class PickupItem : MonoBehaviour {
        [SerializeField] private Item _item;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private void Start() {
            if (_item != null)
                _spriteRenderer.sprite = _item.GetSprite();
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (CanPickUp(other))
                PickUp(other.gameObject);
        }

        protected abstract bool CanPickUp(Collider2D other);

        protected void PickUp(GameObject picker) {
            if (_item != null)
                _item.PerformPickupOperation(picker);

            Destroy(gameObject);
        }

        protected Item GetItem() =>
            _item;
    }
}