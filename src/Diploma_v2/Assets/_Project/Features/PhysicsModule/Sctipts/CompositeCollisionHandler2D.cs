using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.PhysicsModule {
    public class CompositeCollisionHandler2D : CollisionHandler2D {
        [SerializeField] private List<CollisionHandler2D> _collisionHandler2Ds;
        private List<Predicate<Collider2D>> _filters = new();

        private readonly HashSet<Collider2D> _triggerEntered = new();
        private readonly HashSet<Collider2D> _collisionEntered = new();

        public override void ApplyFilter(Predicate<Collider2D> filter) =>
            _filters.Add(filter);

        public override void SetEnabled(bool enabled) =>
            _collisionHandler2Ds.ForEach(handler => handler.SetEnabled(enabled));

        protected override IEnumerable<Collider2D> GetTriggeredColliders() =>
            _triggerEntered;

        protected override IEnumerable<Collider2D> GetCollidedColliders() =>
            _collisionEntered;

        protected override IEnumerable<Collider2D> GetAllSelfTriggerColliders() =>
            _collisionHandler2Ds.SelectMany(collisionHandler2D => collisionHandler2D.SelfTriggerColliders);

        protected override IEnumerable<Collider2D> GetAllSelfCollisionColliders() =>
            _collisionHandler2Ds.SelectMany(collisionHandler2D => collisionHandler2D.SelfCollisionColliders);

        private void OnEnable() {
            foreach (CollisionHandler2D collisionHandler2D in _collisionHandler2Ds) {
                collisionHandler2D.TriggerEntered += OnTriggerEntered;
                collisionHandler2D.TriggerExited += OnTriggerExited;
                collisionHandler2D.CollisionEntered += OnCollisionEntered;
                collisionHandler2D.CollisionExited += OnCollisionExited;
            }
        }

        private void OnDisable() {
            foreach (CollisionHandler2D collisionHandler2D in _collisionHandler2Ds) {
                collisionHandler2D.TriggerEntered -= OnTriggerEntered;
                collisionHandler2D.TriggerExited -= OnTriggerExited;
                collisionHandler2D.CollisionEntered -= OnCollisionEntered;
                collisionHandler2D.CollisionExited -= OnCollisionExited;
            }
        }


        private void OnTriggerEntered(Collider2D obj) {
            if (_filters.Any(filter => !filter(obj)))
                return;
            if (_triggerEntered.Contains(obj))
                return;
            _triggerEntered.Add(obj);
            InvokeTriggerEntered(obj);
        }

        private void OnTriggerExited(Collider2D obj) {
            if (_collisionHandler2Ds.Any(handler => handler.TriggeredColliders.Contains(obj)))
                return;
            if (!_triggerEntered.Contains(obj))
                return;
            _triggerEntered.Remove(obj);
            InvokeTriggerExited(obj);
        }

        private void OnCollisionEntered(Collider2D obj) {
            if (_filters.Any(filter => !filter(obj)))
                return;
            if (_collisionEntered.Contains(obj))
                return;
            _collisionEntered.Add(obj);
            InvokeCollisionEntered(obj);
        }

        private void OnCollisionExited(Collider2D obj) {
            if (_collisionHandler2Ds.Any(handler => handler.CollidedColliders.Contains(obj)))
                return;
            if (!_collisionEntered.Contains(obj))
                return;
            _collisionEntered.Remove(obj);
            InvokeCollisionExited(obj);
        }

        [Button]
        private void GrabCollisionHandlersFromChildren() =>
            _collisionHandler2Ds = GetComponentsInChildren<CollisionHandler2D>().Except(new[] { this }).ToList();
    }
}