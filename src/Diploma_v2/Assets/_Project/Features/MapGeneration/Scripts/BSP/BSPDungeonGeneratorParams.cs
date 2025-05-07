using System;
using Sirenix.OdinInspector;

namespace _Project.Features.MapGeneration.BSP
{
    [Serializable]
    public class BSPDungeonGeneratorParams
    {
        [LabelText("Min Leaf Size")]
        [MinValue(1)]
        [ValidateInput(
            "ValidateLeafSize", 
            "minLeafSize must be greater than minRoomSize + 2 × offsetFromBorders")]
        public int minLeafSize;

        [LabelText("Min Room Size"), MinValue(1)]
        public int minRoomSize;

        [LabelText("Offset From Borders"), MinValue(0)]
        public int offsetFromBorders = 1;

        private bool ValidateLeafSize(int leafSize) =>
            leafSize > minRoomSize + offsetFromBorders * 2;
    }
}