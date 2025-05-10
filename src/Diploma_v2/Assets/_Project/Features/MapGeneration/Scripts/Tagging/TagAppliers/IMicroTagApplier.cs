using _Project.Features.MapGeneration.BSP;
using _Project.Global.Collections;

namespace _Project.Features.MapGeneration.Tagging.TagAppliers {
    public interface IMicroTagApplier {
        public void Apply(PriorityList<IMicroTagRule> rules, Dungeon dungeon, DungeonTags dungeonTags);
    }
}