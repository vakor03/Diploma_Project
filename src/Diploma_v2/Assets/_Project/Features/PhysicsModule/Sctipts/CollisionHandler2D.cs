using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Features.PhysicsModule {
    public abstract class CollisionHandler2D : MonoBehaviour {
        private readonly List<Collider2D> _cachedCollidedColliders = new();
        private readonly List<Collider2D> _cachedTriggeredColliders = new();

        public event Action<Collider2D> TriggerEntered;
        public event Action<Collider2D> TriggerExited;
        public event Action<Collider2D> CollisionEntered;
        public event Action<Collider2D> CollisionExited;
        public event Action<Collider2D> CollisionStay;
        public event Action<Collider2D> TriggerStay;

        public IEnumerable<Collider2D> TriggeredColliders => GetTriggeredColliders();
        public IEnumerable<Collider2D> CollidedColliders => GetCollidedColliders();

        public IEnumerable<Collider2D> AllContactedColliders => GetAllContactedColliders();

        public IEnumerable<Collider2D> AllSelfColliders => GetAllSelfColliders();
        public IEnumerable<Collider2D> SelfTriggerColliders => GetAllSelfTriggerColliders();
        public IEnumerable<Collider2D> SelfCollisionColliders => GetAllSelfCollisionColliders();

        public abstract void ApplyFilter(Predicate<Collider2D> filter);

        public abstract void SetEnabled(bool enabled);

        protected void InvokeTriggerEntered(Collider2D collider2D) =>
            TriggerEntered?.Invoke(collider2D);

        protected void InvokeTriggerExited(Collider2D collider2D) =>
            TriggerExited?.Invoke(collider2D);

        protected void InvokeCollisionEntered(Collider2D collider2D) =>
            CollisionEntered?.Invoke(collider2D);

        protected void InvokeCollisionExited(Collider2D collider2D) =>
            CollisionExited?.Invoke(collider2D);

        protected abstract IEnumerable<Collider2D> GetTriggeredColliders();
        protected abstract IEnumerable<Collider2D> GetCollidedColliders();
        protected abstract IEnumerable<Collider2D> GetAllSelfTriggerColliders();
        protected abstract IEnumerable<Collider2D> GetAllSelfCollisionColliders();

        private IEnumerable<Collider2D> GetAllSelfColliders() {
            HashSet<Collider2D> allSelfColliders = new();
            allSelfColliders.UnionWith(GetAllSelfTriggerColliders());
            allSelfColliders.UnionWith(GetAllSelfCollisionColliders());
            return allSelfColliders;
        }

        private IEnumerable<Collider2D> GetAllContactedColliders() {
            HashSet<Collider2D> allColliders = new();
            allColliders.UnionWith(GetTriggeredColliders());
            allColliders.UnionWith(GetCollidedColliders());
            return allColliders;
        }

        protected void FixedUpdate() {
            _cachedTriggeredColliders.Clear();
            _cachedCollidedColliders.Clear();

            _cachedTriggeredColliders.AddRange(GetTriggeredColliders());
            _cachedCollidedColliders.AddRange(GetCollidedColliders());

            foreach (Collider2D collider2D in _cachedTriggeredColliders)
                TriggerStay?.Invoke(collider2D);
            foreach (Collider2D collider2D in _cachedCollidedColliders)
                CollisionStay?.Invoke(collider2D);
        }
    }
}