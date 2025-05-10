using System.Collections.Generic;

namespace _Project.Features.MapGeneration.Tagging {
    public class MacroTagFilter {
        public HashSet<MacroTag> AllowedTags { get; }

        public MacroTagFilter(params MacroTag[] allowedTags) {
            AllowedTags = new HashSet<MacroTag>(allowedTags);
        }

        public bool IsAllowed(MacroTag tag) {
            return AllowedTags.Count == 0 || AllowedTags.Contains(tag);
        }
    }
}