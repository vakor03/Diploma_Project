using _Project.Extensions.EnumerableExtensions;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.SeedModule;
using _Project.Global.Collections;

namespace _Project.Features.MapGeneration.Tagging.TagAppliers {
    public class GlobalPlaceTagApplier : IGlobalPlaceTagApplier {
        private readonly ISeedService _seedService;

        public GlobalPlaceTagApplier(ISeedService seedService) =>
            _seedService = seedService;

        public void Apply(PriorityList<IGlobalPlaceTagRule> rules, Dungeon dungeon, DungeonTags dungeonTags) {
            foreach (IGlobalPlaceTagRule rule in rules)
                ApplyGlobalTagRule(dungeon, dungeonTags, rule);
        }

        private void ApplyGlobalTagRule(Dungeon dungeon, DungeonTags dungeonTags, IGlobalPlaceTagRule rule) {
            int appliedCount = 0;

            foreach (Room room in dungeon.Rooms.InRandomOrder(_seedService.GetRandom())) {
                if (dungeonTags.GetGlobalPlaceTagForRoom(room) == GlobalPlaceTag.None && rule.IsValidForRoom(room)) {
                    dungeonTags.AddGlobalPlaceTag(rule.GlobalPlaceTag, room);
                    appliedCount++;
                }

                if (appliedCount >= rule.MaxCount)
                    break;
            }
                
            if (appliedCount >= rule.MaxCount)
                return;

            foreach (Tunnel tunnel in dungeon.Tunnels) {
                if (dungeonTags.GetGlobalPlaceTagForTunnel(tunnel) == GlobalPlaceTag.None && rule.IsValidForTunnel(tunnel)) {
                    dungeonTags.AddGlobalPlaceTag(rule.GlobalPlaceTag, tunnel);
                    appliedCount++;
                }

                if (appliedCount >= rule.MaxCount)
                    break;
            }
        }
    }
}