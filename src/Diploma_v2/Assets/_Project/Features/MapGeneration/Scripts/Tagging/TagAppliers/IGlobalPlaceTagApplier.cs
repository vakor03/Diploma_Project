using _Project.Features.MapGeneration.BSP;
using _Project.Global.Collections;

namespace _Project.Features.MapGeneration.Tagging.TagAppliers {
    public interface IGlobalPlaceTagApplier {
        void Apply(PriorityList<IGlobalPlaceTagRule> rules, Dungeon dungeon, DungeonTags dungeonTags);
    }
}