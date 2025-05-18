using System.Collections.Generic;
using UnityEngine;

namespace _Project.Features.LevelGeneratorModule {
    public interface IBlockGroupService {
        void GroupHorizontallyConnectedBlocks(List<Vector2Int> blocks);
        BlockGroup FindGroupContaining(Vector2Int position);
        List<BlockGroup> FindGroupsInRow(int rowY);
    }
}