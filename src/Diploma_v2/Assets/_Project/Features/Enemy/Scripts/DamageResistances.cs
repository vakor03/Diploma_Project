using System;
using AYellowpaper.SerializedCollections;

namespace _Project.Features.Enemy {
    [Serializable]
    public class DamageResistances : SerializedDictionary<DamageType, float> { }
}