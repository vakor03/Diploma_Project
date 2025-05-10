using System.Collections.Generic;
using _Project.Extensions.EnumerableExtensions;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.SeedModule;
using _Project.Global.Collections;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging.TagAppliers {
    public class MacroTagApplier : IMacroTagApplier {
        private readonly ISeedService _seedService;

        public MacroTagApplier(ISeedService seedService) =>
            _seedService = seedService;

        public void Apply(PriorityList<IMacroTagRule> rules, Dungeon dungeon, DungeonTags dungeonTags) {
            Dictionary<GlobalPlaceTag, PriorityList<IMacroTagRule>> rulesByGlobalTag = new();
            PriorityList<IMacroTagRule> rulesForAllTags = new();

            // Group rules by their allowed global place tags for efficiency
            foreach (IMacroTagRule rule in rules) {
                int? priority = rules.GetPriority(rule);
                if (!priority.HasValue) continue;

                if (rule.GlobalPlaceTagFilter.AllowedTags.Count == 0) {
                    rulesForAllTags.Add(rule, priority.Value);
                }
                else {
                    foreach (GlobalPlaceTag tag in rule.GlobalPlaceTagFilter.AllowedTags) {
                        if (!rulesByGlobalTag.ContainsKey(tag))
                            rulesByGlobalTag[tag] = new PriorityList<IMacroTagRule>();
                        rulesByGlobalTag[tag].Add(rule, priority.Value);
                    }
                }
            }

            // Apply macro tags to rooms
            foreach (Room room in dungeon.Rooms.InRandomOrder(_seedService.GetRandom())) {
                GlobalPlaceTag roomGlobalTag = dungeonTags.GetGlobalPlaceTagForRoom(room);

                // Apply rules that work with this specific global tag
                if (rulesByGlobalTag.TryGetValue(roomGlobalTag, out PriorityList<IMacroTagRule> specificRules)) {
                    foreach (IMacroTagRule rule in specificRules) {
                        ApplyMacroTagToRoom(rule, room, roomGlobalTag, dungeonTags);
                    }
                }

                // Apply rules that work with all tags
                foreach (IMacroTagRule rule in rulesForAllTags) {
                    ApplyMacroTagToRoom(rule, room, roomGlobalTag, dungeonTags);
                }
            }

            // Apply macro tags to tunnels
            foreach (Tunnel tunnel in dungeon.Tunnels.InRandomOrder(_seedService.GetRandom())) {
                GlobalPlaceTag tunnelGlobalTag = dungeonTags.GetGlobalPlaceTagForTunnel(tunnel);

                // Apply rules that work with this specific global tag
                if (rulesByGlobalTag.TryGetValue(tunnelGlobalTag, out PriorityList<IMacroTagRule> specificRules)) {
                    foreach (IMacroTagRule rule in specificRules) {
                        ApplyMacroTagToTunnel(rule, tunnel, tunnelGlobalTag, dungeonTags);
                    }
                }

                // Apply rules that work with all tags
                foreach (IMacroTagRule rule in rulesForAllTags) {
                    ApplyMacroTagToTunnel(rule, tunnel, tunnelGlobalTag, dungeonTags);
                }
            }
        }

        private void ApplyMacroTagToRoom(IMacroTagRule rule, Room room, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags) {
            if (!rule.GlobalPlaceTagFilter.IsAllowed(globalPlaceTag))
                return;

            int appliedCount = 0;

            foreach (Vector2Int position in room.Cells.InRandomOrder(_seedService.GetRandom())) {
                if (rule.IsValidForPosition(position, room, globalPlaceTag, dungeonTags)) {
                    dungeonTags.AddMacroTag(rule.MacroTag, position);
                    appliedCount++;
                }

                if (appliedCount >= rule.MaxCount)
                    break;
            }
        }

        private void ApplyMacroTagToTunnel(IMacroTagRule rule, Tunnel tunnel, GlobalPlaceTag globalPlaceTag, DungeonTags dungeonTags) {
            if (!rule.GlobalPlaceTagFilter.IsAllowed(globalPlaceTag))
                return;

            int appliedCount = 0;

            foreach (Vector2Int position in tunnel.Cells.InRandomOrder(_seedService.GetRandom())) {
                if (rule.IsValidForPosition(position, tunnel, globalPlaceTag, dungeonTags)) {
                    dungeonTags.AddMacroTag(rule.MacroTag, position);
                    appliedCount++;
                }

                if (appliedCount >= rule.MaxCount)
                    break;
            }
        }
    }
}