using System.Collections.Generic;
using _Project.Features.MapGeneration.Matrix;
using _Project.Features.SeedModule;
using UnityEngine;

namespace _Project.Features.MapGeneration.BSP {
    public class BSPDungeonGenerator : IMacroLayoutDungeonGenerationService {
        private readonly ISeedService _seedService;

        private Matrix<int> Matrix { get; set; }
        private List<RectInt> Rooms { get; set; }

        private int _minLeafSize;

        public BSPDungeonGenerator(ISeedService seedService) =>
            _seedService = seedService;

        public List<RectInt> Generate(Matrix<int> initialMatrix, BSPDungeonGeneratorParams @params) {
            _minLeafSize = @params.minLeafSize;
            Rooms = new List<RectInt>();

            Matrix = initialMatrix;
            ClearTiles();

            BSPNode root = new(new RectInt(0, 0, Matrix.Width, Matrix.Height));
            List<BSPNode> nodes = new() { root };

            for (int i = 0; i < nodes.Count; i++) {
                BSPNode node = nodes[i];
                if (node.Rect.width > _minLeafSize * 2 || node.Rect.height > _minLeafSize * 2)
                    if (Split(node)) {
                        nodes.Add(node.Left);
                        nodes.Add(node.Right);
                    }
            }

            foreach (BSPNode leaf in nodes)
                if (leaf.IsLeaf)
                    CreateRoom(leaf, @params.minRoomSize, @params.offsetFromBorders);

            CarveRooms();
            return Rooms;
        }

        private void ClearTiles() {
            for (int x = 0; x < Matrix.Width; x++)
            for (int y = 0; y < Matrix.Height; y++)
                Matrix[x, y] = 0;
        }

        private bool Split(BSPNode node) {
            bool horizontal = _seedService.GetRandom().NextDouble() > 0.5;
            if (node.Rect.width > node.Rect.height && node.Rect.height / (float)node.Rect.width < 0.5f)
                horizontal = false;
            else if (node.Rect.height > node.Rect.width && node.Rect.width / (float)node.Rect.height < 0.5f)
                horizontal = true;

            int max = (horizontal ? node.Rect.height : node.Rect.width) - _minLeafSize;
            if (max <= _minLeafSize)
                return false;

            int split = _seedService.GetRandom().Next(_minLeafSize, max);

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

        private void CreateRoom(BSPNode leaf, int minRoomSize, int offset) {
            int roomW = _seedService.GetRandom().Next(minRoomSize, leaf.Rect.width - offset * 2);
            int roomH = _seedService.GetRandom().Next(minRoomSize, leaf.Rect.height - offset * 2);
            int roomX = _seedService.GetRandom().Next(leaf.Rect.x + offset, leaf.Rect.x + leaf.Rect.width - roomW - offset);
            int roomY = _seedService.GetRandom().Next(leaf.Rect.y + offset, leaf.Rect.y + leaf.Rect.height - roomH - offset);

            RectInt room = new RectInt(roomX, roomY, roomW, roomH);
            leaf.Room = room;
            Rooms.Add(room);
        }

        private void CarveRooms() {
            foreach (RectInt room in Rooms)
                for (int x = room.x; x < room.xMax; x++)
                for (int y = room.y; y < room.yMax; y++)
                    Matrix[x, y] = 1;
        }
    }
}