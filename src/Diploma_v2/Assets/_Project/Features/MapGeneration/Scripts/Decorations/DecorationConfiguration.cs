using System.Collections.Generic;
using UnityEngine;

namespace _Project.Features.MapGeneration.Decorations {
    [CreateAssetMenu(fileName = nameof(DecorationConfiguration) +"_Default", menuName = "Configurations/MapGeneration/" + nameof(DecorationConfiguration))]
    public class DecorationConfiguration : ScriptableObject {
        [System.Serializable]
        public class DecorationPrefabSet {
            public List<GameObject> prefabs;
        }

        [Header("Floor Decorations")]
        public DecorationPrefabSet smallFloorDecorations;
        public DecorationPrefabSet bigFloorDecorations;

        [Header("Ceiling Decorations")]
        public DecorationPrefabSet ceilingDecorations;

        [Header("Wall Decorations")]
        public DecorationPrefabSet wallDecorations;

        [Header("Decoration Settings")]
        public float decorationSpacing = 2f;
        public float decorationHeightOffset = 0.5f;
        public float wallDecorationDepth = 0.2f;
    }
} 