using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.MapGeneration.BSP
{
    [Serializable]
    public class BSPDungeonGeneratorParams
    {
        [LabelText("Min Leaf Size")]
        [MinValue(1)]
        // [ValidateInput(
        //     "ValidateLeafSize", 
        //     "minLeafSize must be greater than minRoomSize + 2 × offsetFromBorders")]
        public Vector2Int minLeafSize;

        [LabelText("Min Room Size"), MinValue(1)]
        public Vector2Int minRoomSize;

        [LabelText("Offset From Borders"), MinValue(0)]
        public int offsetFromBorders = 1;

        // private bool ValidateLeafSize(int leafSize) =>
        //     leafSize > minRoomSize + offsetFromBorders * 2;
        
        [Range(0f,1f)]
        public float ShrinkageFactor = 0.5f;
    }
}