using System.Collections.Generic;
using _Project.Features.MapGeneration.Matrix;
using UnityEngine;

namespace _Project.Features.MapGeneration.BSP {
    public class Dungeon {
        public Matrix<int> Matrix;
        public List<RectInt> Rooms;
    }
}