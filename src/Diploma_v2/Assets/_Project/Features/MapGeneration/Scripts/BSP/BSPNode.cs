using UnityEngine;

namespace _Project.Features.MapGeneration.BSP {
    public class BSPNode
    {
        public RectInt Rect;
        public BSPNode Left;
        public BSPNode Right;
        public RectInt? Room;

        public BSPNode(RectInt rect) =>
            Rect = rect;

        public bool IsLeaf => Left == null && Right == null;
    }
}