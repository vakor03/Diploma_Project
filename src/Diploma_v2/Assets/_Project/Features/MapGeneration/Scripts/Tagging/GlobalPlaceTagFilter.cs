using System.Collections.Generic;

namespace _Project.Features.MapGeneration.Tagging {
    public class GlobalPlaceTagFilter {
        public HashSet<GlobalPlaceTag> AllowedTags { get; }

        public GlobalPlaceTagFilter(params GlobalPlaceTag[] allowedTags) {
            AllowedTags = new HashSet<GlobalPlaceTag>(allowedTags);
        }

        public bool IsAllowed(GlobalPlaceTag tag) {
            return AllowedTags.Count == 0 || AllowedTags.Contains(tag);
        }
    }
}