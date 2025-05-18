using System.Collections.Generic;
using _Project.Scripts.Infrastructure;

namespace _Project.Features.LevelGeneratorModule {
    public class BlockGroupsModel : IModel {
        public List<BlockGroup> Groups { get; private set; } = new List<BlockGroup>();

        public void AddGroup(BlockGroup group) =>
            Groups.Add(group);

        public void ClearGroups() =>
            Groups.Clear();
    }
}