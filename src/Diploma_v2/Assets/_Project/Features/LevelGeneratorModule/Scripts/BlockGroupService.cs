using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Features.LevelGeneratorModule {
    public class BlockGroupService : IBlockGroupService {
        private readonly BlockGroupsModel _model;

        public BlockGroupService(BlockGroupsModel model) =>
            _model = model;

        public void GroupHorizontallyConnectedBlocks(List<Vector2Int> blocks) {
            _model.ClearGroups();

            if (blocks == null || blocks.Count == 0)
                return;

            List<Vector2Int> sortedBlocks = blocks.OrderBy(b => b.y).ThenBy(b => b.x).ToList();

            BlockGroup currentGroup = null;
            int currentY = int.MinValue;
            int expectedNextX = int.MinValue;

            foreach (Vector2Int block in sortedBlocks) {
                if (block.y != currentY || block.x != expectedNextX) {
                    if (currentGroup != null && currentGroup.Blocks.Count > 0) {
                        _model.AddGroup(currentGroup);
                    }

                    currentGroup = new BlockGroup();
                    currentGroup.AddBlock(block);
                    currentY = block.y;
                }
                else {
                    currentGroup.AddBlock(block);
                }

                expectedNextX = block.x + 1;
            }

            if (currentGroup != null && currentGroup.Blocks.Count > 0)
                _model.AddGroup(currentGroup);
        }

        public BlockGroup FindGroupContaining(Vector2Int position) {
            foreach (BlockGroup group in _model.Groups)
                if (group.Contains(position))
                    return group;

            return null;
        }

        public List<BlockGroup> FindGroupsInRow(int rowY) {
            return _model.Groups.Where(g => g.Row == rowY).ToList();
        }
    }
}