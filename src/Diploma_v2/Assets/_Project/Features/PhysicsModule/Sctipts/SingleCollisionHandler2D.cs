using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Features.PhysicsModule {
    public class SingleCollisionHandler2D : CollisionHandler2D {
        private readonly HashSet<Collider2D> _triggeredColliders = new();
        private readonly HashSet<Collider2D> _collidedColliders = new();

        private List<Predicate<Collider2D>> _filters = new();

        private Collider2D[] _allColliders;

        private void Awake() =>
            _allColliders = GetComponents<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other) {
            if (_filters.Any(filter => !filter(other)))
                return;
            _triggeredColliders.Add(other);
            InvokeTriggerEntered(other);
        }

        private void OnTriggerExit2D(Collider2D other) {
            _triggeredColliders.Remove(other);
            InvokeTriggerExited(other);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (_filters.Any(filter => !filter(other.collider)))
                return;
            _collidedColliders.Add(other.collider);
            InvokeCollisionEntered(other.collider);
        }

        private void OnCollisionExit2D(Collision2D other) {
            _collidedColliders.Remove(other.collider);
            InvokeCollisionExited(other.collider);
        }

        public override void ApplyFilter(Predicate<Collider2D> filter) =>
            _filters.Add(filter);

        public override void SetEnabled(bool enabled) {
            foreach (Collider2D collider in _allColliders)
                collider.enabled = enabled;
        }

        protected override IEnumerable<Collider2D> GetTriggeredColliders() =>
            _triggeredColliders;

        protected override IEnumerable<Collider2D> GetCollidedColliders() =>
            _collidedColliders;

        protected override IEnumerable<Collider2D> GetAllSelfTriggerColliders() =>
            _allColliders.Where(col => col.isTrigger);

        protected override IEnumerable<Collider2D> GetAllSelfCollisionColliders() =>
            _allColliders.Where(col => !col.isTrigger);
    }
}