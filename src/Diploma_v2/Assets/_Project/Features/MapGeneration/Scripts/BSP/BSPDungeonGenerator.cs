using System.Collections.Generic;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.SeedModule;
using UnityEngine;

namespace _Project.Features.MapGeneration.BSP {
    public class BSPDungeonGenerator : IMacroLayoutDungeonGenerationService {
        private readonly ISeedService _seedService;

        // private Matrix<BlockType> Matrix { get; set; }
        private List<RectInt> Rooms { get; set; }

        private Vector2 _minLeafSize;

        public BSPDungeonGenerator(ISeedService seedService) =>
            _seedService = seedService;

        public List<RectInt> Generate(Vector2Int matrixSize, BSPDungeonGeneratorParams @params) {
            _minLeafSize = @params.minLeafSize;
            Rooms = new List<RectInt>();

            // Matrix = initialMatrix;

            BSPNode root = new(new RectInt(0, 0, matrixSize.x, matrixSize.y));
            List<BSPNode> nodes = new() { root };

            for (int i = 0; i < nodes.Count; i++) {
                BSPNode node = nodes[i];
                if (node.Rect.width > _minLeafSize.x * 2 || node.Rect.height > _minLeafSize.y * 2)
                    if (Split(node)) {
                        nodes.Add(node.Left);
                        nodes.Add(node.Right);
                    }
            }

            foreach (BSPNode leaf in nodes)
                if (leaf.IsLeaf)
                    CreateRoom(leaf, @params.minRoomSize, @params.offsetFromBorders, @params.ShrinkageFactor);

            return Rooms;
        }

        private bool Split(BSPNode node) {
            bool horizontal = _seedService.GetRandom().NextDouble() > 0.5;
            if (node.Rect.width > node.Rect.height && node.Rect.height / (float)node.Rect.width < 0.5f)
                horizontal = false;
            else if (node.Rect.height > node.Rect.width && node.Rect.width / (float)node.Rect.height < 0.5f)
                horizontal = true;

            // Use the appropriate dimension of _minLeafSize based on split direction
            float minSize = horizontal ? _minLeafSize.y : _minLeafSize.x;
            int max = (horizontal ? node.Rect.height : node.Rect.width) - Mathf.FloorToInt(minSize);
            
            if (max <= minSize)
                return false;

            int split = _seedService.GetRandom().Next(Mathf.FloorToInt(minSize), max);

            if (horizontal) {
                node.Left = new BSPNode(new RectInt(node.Rect.x, node.Rect.y, node.Rect.width, split));
                node.Right = new BSPNode(new RectInt(node.Rect.x, node.Rect.y + split, node.Rect.width, node.Rect.height - split));
            }
            else {
                node.Left = new BSPNode(new RectInt(node.Rect.x, node.Rect.y, split, node.Rect.height));
                node.Right = new BSPNode(new RectInt(node.Rect.x + split, node.Rect.y, node.Rect.width - split, node.Rect.height));
            }

            return true;
        }

        private void CreateRoom(BSPNode leaf, Vector2 minRoomSize, int offset, float shrinkageRate) {
            int roomW = _seedService.GetRandom().Next((int)(leaf.Rect.width * (1 - shrinkageRate)), leaf.Rect.width);
            int roomH = _seedService.GetRandom().Next((int)(leaf.Rect.height * (1 - shrinkageRate)), leaf.Rect.height);
            
            // Apply minimum dimensions from Vector2 minRoomSize
            roomW = Mathf.Max(roomW, Mathf.FloorToInt(minRoomSize.x));
            roomH = Mathf.Max(roomH, Mathf.FloorToInt(minRoomSize.y));
            
            // Calculate room position
            int roomX = _seedService.GetRandom().Next(leaf.Rect.x + offset, leaf.Rect.x + leaf.Rect.width - roomW - offset);
            int roomY = _seedService.GetRandom().Next(leaf.Rect.y + offset, leaf.Rect.y + leaf.Rect.height - roomH - offset);

            RectInt room = new RectInt(roomX, roomY, roomW, roomH);
            leaf.Room = room;
            Rooms.Add(room);
        }
    }
}