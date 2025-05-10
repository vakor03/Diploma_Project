using System;
using UnityEngine;

namespace _Project.Features.ExperienceModule {
    [Serializable]
    public class XPOrbData
    {
        public XPOrbType orbType;
        public float minXP;
        public float maxXP;
        public GameObject prefab;
        public Color orbColor = Color.white;
    
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float dissolveDuration = 0.3f;
    }
}