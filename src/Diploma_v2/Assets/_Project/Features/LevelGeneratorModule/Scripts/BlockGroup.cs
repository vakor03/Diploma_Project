using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Features.LevelGeneratorModule {
    public class BlockGroup {
        public List<Vector2Int> Blocks { get; private set; }
        public RectInt Bounds { get; private set; }
        public int Row { get; private set; }

        private HashSet<Vector2Int> _blockSet;

        public BlockGroup() {
            Blocks = new List<Vector2Int>();
            _blockSet = new HashSet<Vector2Int>();
            Bounds = new RectInt(0, 0, 0, 0);
        }

        public BlockGroup(IEnumerable<Vector2Int> blocks) {
            Blocks = new List<Vector2Int>(blocks);
            _blockSet = new HashSet<Vector2Int>(blocks);

            if (Blocks.Count > 0) {
                Row = Blocks[0].y;

                int minX = Blocks.Min(b => b.x);
                int maxX = Blocks.Max(b => b.x);

                Bounds = new RectInt(minX, Row, maxX - minX + 1, 1);
            }
        }

        public void AddBlock(Vector2Int block) {
            if (Blocks.Count == 0) {
                Row = block.y;
                Bounds = new RectInt(block.x, block.y, 1, 1);
            }
            else if (block.y != Row) {
                throw new System.ArgumentException("Cannot add block with different Y coordinate to horizontal group");
            }

            Blocks.Add(block);
            _blockSet.Add(block);

            if (block.x < Bounds.x) {
                int increase = Bounds.x - block.x;
                RectInt bounds = Bounds;
                bounds.x = block.x;
                bounds.width += increase;
                Bounds = bounds;
            }
            else if (block.x >= Bounds.x + Bounds.width) {
                RectInt bounds = Bounds;
                bounds.width = block.x - Bounds.x + 1;
                Bounds = bounds;
            }
        }

        public bool Contains(Vector2Int position) {
            if (!Bounds.Contains(position))
                return false;

            return _blockSet.Contains(position);
        }

        public bool CanAddBlock(Vector2Int block) {
            if (Blocks.Count == 0)
                return true;

            if (block.y != Row)
                return false;

            return _blockSet.Contains(new Vector2Int(block.x - 1, block.y)) ||
                   _blockSet.Contains(new Vector2Int(block.x + 1, block.y));
        }

        public bool IsContiguous() {
            if (Blocks.Count <= 1)
                return true;

            return Bounds.width == Blocks.Count;
        }
    }
}