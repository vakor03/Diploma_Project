using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Matrix;
using UnityEngine;

namespace _Project.Features.MapGeneration.CA {
    public interface ICaveRoomCarveService {
        public Room CarveRoom(Matrix<int> matrix, RectInt region, CAConfig config);
    }
}