using System;
using UnityEngine;

namespace _Project.Features.Enemy {
    [Serializable]
    public class DeathArgs
    {
        public Transform Killer;
        public bool IsInstantDeath;
        public Transform EntityTransform;
    }
}