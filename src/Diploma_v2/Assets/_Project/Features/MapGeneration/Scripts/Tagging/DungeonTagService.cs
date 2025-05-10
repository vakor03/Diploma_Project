// Main DungeonTagService.cs

using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Tagging.TagAppliers;
using _Project.Global.Collections;

namespace _Project.Features.MapGeneration.Tagging {
    public class DungeonTagService : IDungeonTagService {
        private readonly IGlobalPlaceTagApplier _globalPlaceTagApplier;
        private readonly IMacroTagApplier _macroTagApplier;
        private readonly IMicroTagApplier _microTagApplier;

        public DungeonTagService(
            IGlobalPlaceTagApplier globalPlaceTagApplier,
            IMacroTagApplier macroTagApplier,
            IMicroTagApplier microTagApplier) {
            _globalPlaceTagApplier = globalPlaceTagApplier;
            _macroTagApplier = macroTagApplier;
            _microTagApplier = microTagApplier;
        }

        public DungeonTags TagAllRegions(Dungeon dungeon,
                                         PriorityList<IGlobalPlaceTagRule> globalPlaceTagRules,
                                         PriorityList<IMacroTagRule> macroTagRules,
                                         PriorityList<IMicroTagRule> microTagRules) {
            DungeonTags dungeonTags = new();

            SetContextForRules(globalPlaceTagRules, macroTagRules, microTagRules, dungeon);

            _globalPlaceTagApplier.Apply(globalPlaceTagRules, dungeon, dungeonTags);
            _macroTagApplier.Apply(macroTagRules, dungeon, dungeonTags);
            _microTagApplier.Apply(microTagRules, dungeon, dungeonTags);

            return dungeonTags;
        }

        private void SetContextForRules(PriorityList<IGlobalPlaceTagRule> globalRules,
                                        PriorityList<IMacroTagRule> macroRules,
                                        PriorityList<IMicroTagRule> microRules,
                                        Dungeon dungeon) {
            foreach (IGlobalPlaceTagRule rule in globalRules)
                rule.Context = dungeon;
            foreach (IMacroTagRule rule in macroRules)
                rule.Context = dungeon;
            foreach (IMicroTagRule rule in microRules)
                rule.Context = dungeon;
        }
    }
}