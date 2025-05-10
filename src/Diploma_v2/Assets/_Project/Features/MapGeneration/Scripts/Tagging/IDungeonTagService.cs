using System.Collections.Generic;
using _Project.Features.MapGeneration.BSP;
using _Project.Global.Collections;

namespace _Project.Features.MapGeneration.Tagging {
    public interface IDungeonTagService {
        public DungeonTags TagAllRegions(Dungeon dungeon,
                                         PriorityList<IGlobalPlaceTagRule> globalPlaceTagRules,
                                         PriorityList<IMacroTagRule> macroTagRules,
                                         PriorityList<IMicroTagRule> microTagRules);
    }
}