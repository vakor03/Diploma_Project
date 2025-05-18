using System;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEngine;

namespace _Project.Features.LevelGeneratorModule {
    [Serializable]
    public class TilemapColliderConfiguration {
        [field: SerializeField] public CompositeCollider2D CompositeCollider { get; private set; }
        [field: SerializeField] public Collider2D SingleCollider { get; private set; }
        [field: SerializeField] public Layer Layer { get; private set; }
    }
}