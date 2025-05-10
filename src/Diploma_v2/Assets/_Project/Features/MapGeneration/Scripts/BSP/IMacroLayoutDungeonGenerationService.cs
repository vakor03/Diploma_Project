using System.Collections.Generic;
using _Project.Features.MapGeneration.Matrix;
using UnityEngine;

namespace _Project.Features.MapGeneration.BSP {
    public interface IMacroLayoutDungeonGenerationService {
        public List<RectInt> Generate(Matrix<BlockType> initialMatrix, BSPDungeonGeneratorParams @params);
    }
}