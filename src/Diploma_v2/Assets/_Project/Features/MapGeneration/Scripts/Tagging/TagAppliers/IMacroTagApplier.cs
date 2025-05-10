using _Project.Features.MapGeneration.BSP;
using _Project.Global.Collections;

namespace _Project.Features.MapGeneration.Tagging.TagAppliers {
    public interface IMacroTagApplier {
        void Apply(PriorityList<IMacroTagRule> rules, Dungeon dungeon, DungeonTags dungeonTags);
    }
}