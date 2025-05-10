using System.Collections.Generic;
using _Project.Extensions.EnumerableExtensions;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.SeedModule;
using _Project.Global.Collections;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging.TagAppliers {
    public class MicroTagApplier : IMicroTagApplier {
        private readonly ISeedService _seedService;

        public MicroTagApplier(ISeedService seedService) =>
            _seedService = seedService;

        public void Apply(PriorityList<IMicroTagRule> rules, Dungeon dungeon, DungeonTags dungeonTags) {
            Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts = InitializeRuleCounts(rules);
            RuleGroups ruleGroups = GroupRulesByFilters(rules);
            
            ApplyToRooms(dungeon, dungeonTags, ruleGroups, ruleCounts);
            ApplyToTunnels(dungeon, dungeonTags, ruleGroups, ruleCounts);
        }

        private Dictionary<IMicroTagRule, MicroTagRuleCounts> InitializeRuleCounts(PriorityList<IMicroTagRule> rules) {
            Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts = new();
            
            foreach (IMicroTagRule rule in rules)
                ruleCounts[rule] = new MicroTagRuleCounts();

            return ruleCounts;
        }

        private RuleGroups GroupRulesByFilters(PriorityList<IMicroTagRule> rules) {
            RuleGroups groups = new();

            foreach (IMicroTagRule rule in rules) {
                int? priority = rules.GetPriority(rule);
                if (!priority.HasValue) continue;

                CategorizeRule(rule, priority.Value, groups);
            }

            return groups;
        }

        private void CategorizeRule(IMicroTagRule rule, int priority, RuleGroups groups) {
            bool hasGlobalFilter = rule.GlobalPlaceTagFilter.AllowedTags.Count > 0;
            bool hasMacroFilter = rule.MacroTagFilter.AllowedTags.Count > 0;

            if (!hasGlobalFilter && !hasMacroFilter) {
                AddRuleToAllTags(rule, priority, groups);
            }
            else if (hasGlobalFilter && hasMacroFilter) {
                AddRuleWithBothFilters(rule, priority, groups);
            }
            else if (hasGlobalFilter) {
                AddRuleWithGlobalFilter(rule, priority, groups);
            }
            else {
                AddRuleWithMacroFilter(rule, priority, groups);
            }
        }

        private void AddRuleToAllTags(IMicroTagRule rule, int priority, RuleGroups groups) {
            groups.RulesForAllTags.Add(rule, priority);
        }

        private void AddRuleWithBothFilters(IMicroTagRule rule, int priority, RuleGroups groups) {
            foreach (GlobalPlaceTag globalTag in rule.GlobalPlaceTagFilter.AllowedTags) {
                foreach (MacroTag macroTag in rule.MacroTagFilter.AllowedTags) {
                    (GlobalPlaceTag, MacroTag) key = (globalTag, macroTag);
                    EnsureKeyExists(key, groups.RulesByTags);
                    groups.RulesByTags[key].Add(rule, priority);
                }
            }
        }

        private void AddRuleWithGlobalFilter(IMicroTagRule rule, int priority, RuleGroups groups) {
            foreach (GlobalPlaceTag globalTag in rule.GlobalPlaceTagFilter.AllowedTags) {
                EnsureKeyExists(globalTag, groups.RulesByGlobalTag);
                groups.RulesByGlobalTag[globalTag].Add(rule, priority);
            }
        }

        private void AddRuleWithMacroFilter(IMicroTagRule rule, int priority, RuleGroups groups) {
            foreach (MacroTag macroTag in rule.MacroTagFilter.AllowedTags) {
                EnsureKeyExists(macroTag, groups.RulesByMacroTag);
                groups.RulesByMacroTag[macroTag].Add(rule, priority);
            }
        }

        private void EnsureKeyExists<TKey>(TKey key, Dictionary<TKey, PriorityList<IMicroTagRule>> dictionary) {
            if (!dictionary.ContainsKey(key))
                dictionary[key] = new PriorityList<IMicroTagRule>();
        }

        private void ApplyToRooms(Dungeon dungeon, DungeonTags dungeonTags, RuleGroups ruleGroups, 
                                  Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            foreach (Room room in dungeon.Rooms.InRandomOrder(_seedService.GetRandom()))
                ProcessRoom(room, dungeonTags, ruleGroups, ruleCounts);
        }

        private void ProcessRoom(Room room, DungeonTags dungeonTags, RuleGroups ruleGroups,
                                 Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            GlobalPlaceTag roomGlobalTag = dungeonTags.GetGlobalPlaceTagForRoom(room);
            Dictionary<IMicroTagRule, int> localCounts = new();

            foreach (Vector2Int position in room.Cells.InRandomOrder(_seedService.GetRandom())) {
                if (dungeonTags.GetMicroTag(position) != MicroTag.None) continue;
                ProcessRoomPosition(position, room, roomGlobalTag, dungeonTags, ruleGroups, localCounts, ruleCounts);
            }
        }

        private void ProcessRoomPosition(Vector2Int position, Room room, GlobalPlaceTag roomGlobalTag,
                                         DungeonTags dungeonTags, RuleGroups ruleGroups,
                                         Dictionary<IMicroTagRule, int> localCounts,
                                         Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            MacroTag macroTag = dungeonTags.GetMacroTag(position);

            if (TryApplySpecificRules(position, room, roomGlobalTag, macroTag, dungeonTags, 
                                      ruleGroups, localCounts, ruleCounts)) return;

            if (TryApplyGlobalOnlyRules(position, room, roomGlobalTag, macroTag, dungeonTags, 
                                        ruleGroups, localCounts, ruleCounts)) return;

            if (TryApplyMacroOnlyRules(position, room, roomGlobalTag, macroTag, dungeonTags, 
                                       ruleGroups, localCounts, ruleCounts)) return;

            TryApplyUnfilteredRules(position, room, roomGlobalTag, macroTag, dungeonTags, 
                                    ruleGroups, localCounts, ruleCounts);
        }

        private bool TryApplySpecificRules(Vector2Int position, Room room, GlobalPlaceTag roomGlobalTag, MacroTag macroTag,
                                           DungeonTags dungeonTags, RuleGroups ruleGroups,
                                           Dictionary<IMicroTagRule, int> localCounts,
                                           Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (ruleGroups.RulesByTags.TryGetValue((roomGlobalTag, macroTag), out PriorityList<IMicroTagRule> combinedRules)) {
                foreach (IMicroTagRule rule in combinedRules) {
                    if (TryApplyMicroTagToPosition(rule, position, room, roomGlobalTag, macroTag, dungeonTags, localCounts, ruleCounts))
                        return true;
                }
            }
            return false;
        }

        private bool TryApplyGlobalOnlyRules(Vector2Int position, Room room, GlobalPlaceTag roomGlobalTag, MacroTag macroTag,
                                             DungeonTags dungeonTags, RuleGroups ruleGroups,
                                             Dictionary<IMicroTagRule, int> localCounts,
                                             Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (dungeonTags.GetMicroTag(position) != MicroTag.None) return false;

            if (ruleGroups.RulesByGlobalTag.TryGetValue(roomGlobalTag, out PriorityList<IMicroTagRule> globalRules)) {
                foreach (IMicroTagRule rule in globalRules) {
                    if (rule.MacroTagFilter.IsAllowed(macroTag) &&
                        TryApplyMicroTagToPosition(rule, position, room, roomGlobalTag, macroTag, dungeonTags, localCounts, ruleCounts))
                        return true;
                }
            }
            return false;
        }

        private bool TryApplyMacroOnlyRules(Vector2Int position, Room room, GlobalPlaceTag roomGlobalTag, MacroTag macroTag,
                                            DungeonTags dungeonTags, RuleGroups ruleGroups,
                                            Dictionary<IMicroTagRule, int> localCounts,
                                            Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (dungeonTags.GetMicroTag(position) != MicroTag.None) return false;

            if (ruleGroups.RulesByMacroTag.TryGetValue(macroTag, out PriorityList<IMicroTagRule> macroRules)) {
                foreach (IMicroTagRule rule in macroRules) {
                    if (rule.GlobalPlaceTagFilter.IsAllowed(roomGlobalTag) &&
                        TryApplyMicroTagToPosition(rule, position, room, roomGlobalTag, macroTag, dungeonTags, localCounts, ruleCounts))
                        return true;
                }
            }
            return false;
        }

        private bool TryApplyUnfilteredRules(Vector2Int position, Room room, GlobalPlaceTag roomGlobalTag, MacroTag macroTag,
                                             DungeonTags dungeonTags, RuleGroups ruleGroups,
                                             Dictionary<IMicroTagRule, int> localCounts,
                                             Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (dungeonTags.GetMicroTag(position) != MicroTag.None) return false;

            foreach (IMicroTagRule rule in ruleGroups.RulesForAllTags) {
                if (TryApplyMicroTagToPosition(rule, position, room, roomGlobalTag, macroTag, dungeonTags, localCounts, ruleCounts))
                    return true;
            }
            return false;
        }

        private void ApplyToTunnels(Dungeon dungeon, DungeonTags dungeonTags, RuleGroups ruleGroups,
                                    Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            RuleGroups tunnelGroups = GroupRulesByFilters(ruleGroups.GetAllRules());

            foreach (Tunnel tunnel in dungeon.Tunnels.InRandomOrder(_seedService.GetRandom())) {
                ProcessTunnel(tunnel, dungeon, dungeonTags, tunnelGroups, ruleCounts);
            }
        }

        private void ProcessTunnel(Tunnel tunnel, Dungeon dungeon, DungeonTags dungeonTags, RuleGroups tunnelGroups,
                                   Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            GlobalPlaceTag tunnelGlobalTag = dungeonTags.GetGlobalPlaceTagForTunnel(tunnel);
            Dictionary<IMicroTagRule, int> localCounts = new();

            foreach (Vector2Int position in tunnel.Cells.InRandomOrder(_seedService.GetRandom())) {
                ProcessTunnelPosition(position, tunnel, tunnelGlobalTag, dungeonTags, tunnelGroups, localCounts, ruleCounts);
            }
        }

        private void ProcessTunnelPosition(Vector2Int position, Tunnel tunnel, GlobalPlaceTag tunnelGlobalTag,
                                           DungeonTags dungeonTags, RuleGroups tunnelGroups,
                                           Dictionary<IMicroTagRule, int> localCounts,
                                           Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            MacroTag macroTag = dungeonTags.GetMacroTag(position);

            if (TryApplySpecificTunnelRules(position, tunnel, tunnelGlobalTag, macroTag, dungeonTags, 
                                            tunnelGroups, localCounts, ruleCounts)) return;

            if (TryApplyGlobalOnlyTunnelRules(position, tunnel, tunnelGlobalTag, macroTag, dungeonTags, 
                                              tunnelGroups, localCounts, ruleCounts)) return;

            if (TryApplyMacroOnlyTunnelRules(position, tunnel, tunnelGlobalTag, macroTag, dungeonTags, 
                                             tunnelGroups, localCounts, ruleCounts)) return;

            TryApplyUnfilteredTunnelRules(position, tunnel, tunnelGlobalTag, macroTag, dungeonTags, 
                                          tunnelGroups, localCounts, ruleCounts);
        }

        private bool TryApplySpecificTunnelRules(Vector2Int position, Tunnel tunnel, GlobalPlaceTag tunnelGlobalTag, MacroTag macroTag,
                                                 DungeonTags dungeonTags, RuleGroups tunnelGroups,
                                                 Dictionary<IMicroTagRule, int> localCounts,
                                                 Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (tunnelGroups.RulesByTags.TryGetValue((tunnelGlobalTag, macroTag), out PriorityList<IMicroTagRule> combinedRules)) {
                foreach (IMicroTagRule rule in combinedRules) {
                    if (TryApplyMicroTagToTunnelPosition(rule, position, tunnel, tunnelGlobalTag, macroTag, dungeonTags, localCounts, ruleCounts))
                        return true;
                }
            }
            return false;
        }

        private bool TryApplyGlobalOnlyTunnelRules(Vector2Int position, Tunnel tunnel, GlobalPlaceTag tunnelGlobalTag, MacroTag macroTag,
                                                   DungeonTags dungeonTags, RuleGroups tunnelGroups,
                                                   Dictionary<IMicroTagRule, int> localCounts,
                                                   Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (dungeonTags.GetMicroTag(position) != MicroTag.None) return false;

            if (tunnelGroups.RulesByGlobalTag.TryGetValue(tunnelGlobalTag, out PriorityList<IMicroTagRule> globalRules)) {
                foreach (IMicroTagRule rule in globalRules) {
                    if (rule.MacroTagFilter.IsAllowed(macroTag) &&
                        TryApplyMicroTagToTunnelPosition(rule, position, tunnel, tunnelGlobalTag, macroTag, dungeonTags, localCounts, ruleCounts))
                        return true;
                }
            }
            return false;
        }

        private bool TryApplyMacroOnlyTunnelRules(Vector2Int position, Tunnel tunnel, GlobalPlaceTag tunnelGlobalTag, MacroTag macroTag,
                                                  DungeonTags dungeonTags, RuleGroups tunnelGroups,
                                                  Dictionary<IMicroTagRule, int> localCounts,
                                                  Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (dungeonTags.GetMicroTag(position) != MicroTag.None) return false;

            if (tunnelGroups.RulesByMacroTag.TryGetValue(macroTag, out PriorityList<IMicroTagRule> macroRules)) {
                foreach (IMicroTagRule rule in macroRules) {
                    if (rule.GlobalPlaceTagFilter.IsAllowed(tunnelGlobalTag) &&
                        TryApplyMicroTagToTunnelPosition(rule, position, tunnel, tunnelGlobalTag, macroTag, dungeonTags, localCounts, ruleCounts))
                        return true;
                }
            }
            return false;
        }

        private bool TryApplyUnfilteredTunnelRules(Vector2Int position, Tunnel tunnel, GlobalPlaceTag tunnelGlobalTag, MacroTag macroTag,
                                                   DungeonTags dungeonTags, RuleGroups tunnelGroups,
                                                   Dictionary<IMicroTagRule, int> localCounts,
                                                   Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (dungeonTags.GetMicroTag(position) != MicroTag.None) return false;

            foreach (IMicroTagRule rule in tunnelGroups.RulesForAllTags) {
                if (TryApplyMicroTagToTunnelPosition(rule, position, tunnel, tunnelGlobalTag, macroTag, dungeonTags, localCounts, ruleCounts))
                    return true;
            }
            return false;
        }

        private bool TryApplyMicroTagToTunnelPosition(IMicroTagRule rule, Vector2Int position, Tunnel tunnel,
                                                      GlobalPlaceTag globalPlaceTag, MacroTag macroTag, DungeonTags dungeonTags,
                                                      Dictionary<IMicroTagRule, int> localCounts,
                                                      Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (ruleCounts[rule].GlobalCount >= rule.GlobalMaxCount) return false;

            InitializeLocalCount(rule, localCounts);

            if (localCounts[rule] >= rule.LocalMaxCount) return false;

            if (rule.IsValidForPosition(position, tunnel, globalPlaceTag, macroTag, dungeonTags)) {
                ApplyTag(rule, position, dungeonTags, localCounts, ruleCounts);
                return true;
            }

            return false;
        }

        private bool TryApplyMicroTagToPosition(IMicroTagRule rule, Vector2Int position, Room room,
                                                GlobalPlaceTag globalPlaceTag, MacroTag macroTag, DungeonTags dungeonTags,
                                                Dictionary<IMicroTagRule, int> localCounts,
                                                Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            if (ruleCounts[rule].GlobalCount >= rule.GlobalMaxCount) return false;

            InitializeLocalCount(rule, localCounts);

            if (localCounts[rule] >= rule.LocalMaxCount) return false;

            if (rule.IsValidForPosition(position, room, globalPlaceTag, macroTag, dungeonTags)) {
                ApplyTag(rule, position, dungeonTags, localCounts, ruleCounts);
                return true;
            }

            return false;
        }

        private void InitializeLocalCount(IMicroTagRule rule, Dictionary<IMicroTagRule, int> localCounts) {
            if (!localCounts.ContainsKey(rule)) {
                localCounts[rule] = 0;
            }
        }

        private void ApplyTag(IMicroTagRule rule, Vector2Int position, DungeonTags dungeonTags,
                              Dictionary<IMicroTagRule, int> localCounts,
                              Dictionary<IMicroTagRule, MicroTagRuleCounts> ruleCounts) {
            dungeonTags.AddMicroTag(rule.MicroTag, position);
            localCounts[rule]++;
            ruleCounts[rule].GlobalCount++;
        }
    }

    public class MicroTagRuleCounts {
        public int GlobalCount { get; set; }
    }

    public class RuleGroups {
        public Dictionary<(GlobalPlaceTag, MacroTag), PriorityList<IMicroTagRule>> RulesByTags { get; } = new();
        public Dictionary<GlobalPlaceTag, PriorityList<IMicroTagRule>> RulesByGlobalTag { get; } = new();
        public Dictionary<MacroTag, PriorityList<IMicroTagRule>> RulesByMacroTag { get; } = new();
        public PriorityList<IMicroTagRule> RulesForAllTags { get; } = new();

        public PriorityList<IMicroTagRule> GetAllRules() {
            PriorityList<IMicroTagRule> allRules = new();

            AddRulesFromDictionary(RulesByTags.Values, allRules);
            AddRulesFromDictionary(RulesByGlobalTag.Values, allRules);
            AddRulesFromDictionary(RulesByMacroTag.Values, allRules);
            AddRulesFromCollection(RulesForAllTags, allRules);

            return allRules;
        }

        private void AddRulesFromDictionary<TKey>(Dictionary<TKey, PriorityList<IMicroTagRule>>.ValueCollection collections,
                                                  PriorityList<IMicroTagRule> allRules) {
            foreach (var rules in collections) {
                AddRulesFromCollection(rules, allRules);
            }
        }

        private void AddRulesFromCollection(PriorityList<IMicroTagRule> sourceRules, PriorityList<IMicroTagRule> targetRules) {
            foreach (IMicroTagRule rule in sourceRules) {
                int? priority = sourceRules.GetPriority(rule);
                if (priority.HasValue) {
                    targetRules.Add(rule, priority.Value);
                }
            }
        }
    }
}