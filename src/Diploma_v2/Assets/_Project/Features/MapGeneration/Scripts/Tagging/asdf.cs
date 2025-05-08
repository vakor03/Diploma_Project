using System;
using System.Collections.Generic;
using _Project.Features.MapGeneration.Matrix;
using UnityEngine;

namespace _Project.Features.MapGeneration.Tagging {
    // Tags for basic region sizes
    public enum RegionTag {
        TinyRoom,
        SmallRoom,
        LargeRoom,
        Corridor
    }

    // Tags for smaller subdivisions
    public enum SubregionTag {
        Platform,
        LightZone
    }

    // Interface for all tagging rules
    public interface ITaggingRule {
        int Priority { get; }
        bool IsMatch(List<Vector2Int> cells, RegionTag? regionContext);
        RegionTag? RegionTag { get; }
        SubregionTag? SubregionTag { get; }
    }

    // Default implementation of a tagging rule
    public class TaggingRule : ITaggingRule {
        public int Priority { get; set; }
        public Func<List<Vector2Int>, RegionTag?, bool> Predicate { get; set; }
        public RegionTag? RegionTag { get; set; }
        public SubregionTag? SubregionTag { get; set; }
        public bool IsMatch(List<Vector2Int> cells, RegionTag? regionContext) {
            return Predicate(cells, regionContext);
        }
    }

    // Holds a tagged region of map cells for placement
    public class PlacementRegion {
        public RegionTag RegionTag { get; set; }
        public SubregionTag? SubregionTag { get; set; }
        public List<Vector2Int> Cells { get; set; }
    }

    // Configuration thresholds for tagging
    public class TaggingConfig {
        public int SmallRoomThreshold { get; set; }
        public int LargeRoomThreshold { get; set; }
        public int LightZoneSpacing { get; set; }
        public int PlatformMinLength { get; set; }
    }

    // Extracts platform and light subregions directly from a list of cells
    public static class SubregionExtractor {
        public static List<List<Vector2Int>> FindPlatformSegments(List<Vector2Int> cells, Matrix<int> matrix) {
            var segments = new List<List<Vector2Int>>();
            var rows = new Dictionary<int, List<int>>();
            foreach (var cell in cells) {
                int x = cell.x, y = cell.y;
                if (matrix[x, y] != 1) continue;
                if (y + 1 < matrix.Height && matrix[x, y + 1] == 1) continue;
                if (y - 1 >= 0 && matrix[x, y - 1] != 1) continue;
                if (!rows.ContainsKey(y)) rows[y] = new List<int>();
                rows[y].Add(x);
            }
            foreach (var kv in rows) {
                var xs = kv.Value;
                xs.Sort();
                var run = new List<Vector2Int>();
                for (int i = 0; i < xs.Count; i++) {
                    int currentX = xs[i];
                    if (i > 0 && currentX != xs[i - 1] + 1) {
                        segments.Add(run);
                        run = new List<Vector2Int>();
                    }
                    run.Add(new Vector2Int(currentX, kv.Key));
                }
                if (run.Count > 0) segments.Add(run);
            }
            return segments;
        }

        public static List<List<Vector2Int>> FindLightZones(List<Vector2Int> cells, int spacing) {
            var zones = new List<List<Vector2Int>>();
            var bounds = ComputeBounds(cells);
            for (int gx = bounds.xMin; gx <= bounds.xMax; gx += spacing) {
                for (int gy = bounds.yMin; gy <= bounds.yMax; gy += spacing) {
                    var zone = new List<Vector2Int>();
                    foreach (var cell in cells) {
                        if (cell.x >= gx && cell.x < gx + spacing &&
                            cell.y >= gy && cell.y < gy + spacing) {
                            zone.Add(cell);
                        }
                    }
                    if (zone.Count > 0) zones.Add(zone);
                }
            }
            return zones;
        }

        static RectInt ComputeBounds(List<Vector2Int> cells) {
            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;
            foreach (var c in cells) {
                if (c.x < minX) minX = c.x;
                if (c.x > maxX) maxX = c.x;
                if (c.y < minY) minY = c.y;
                if (c.y > maxY) maxY = c.y;
            }
            return new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }
    }

    // Non-static tagger applies rules to generate PlacementRegions

    // Builder for PlacementTagger to add custom rules
    // public class PlacementTaggerBuilder {
    //     private readonly TaggingConfig _config;
    //     private readonly List<ITaggingRule> _rules = new List<ITaggingRule>();
    //     public PlacementTaggerBuilder(TaggingConfig config) {
    //         _config = config;
    //         // default region rules
    //         AddRegionRule(100, RegionTag.LargeRoom, (cells, ctx) => cells.Count >= config.LargeRoomThreshold);
    //         AddRegionRule(80, RegionTag.SmallRoom, (cells, ctx) => cells.Count >= config.SmallRoomThreshold);
    //         AddRegionRule(60, RegionTag.TinyRoom, (cells, ctx) => true);
    //         // default subregion rules
    //         AddSubregionRule(50, SubregionTag.Platform, (cells, ctx) => cells.Count >= config.PlatformMinLength);
    //         AddSubregionRule(30, SubregionTag.LightZone, (cells, ctx) => cells.Count > 0);
    //     }
    //     public PlacementTaggerBuilder AddRule(ITaggingRule rule) { _rules.Add(rule); return this; }
    //     public PlacementTaggerBuilder AddRegionRule(int priority, RegionTag tag, Func<List<Vector2Int>, RegionTag?, bool> pred) {
    //         _rules.Add(new TaggingRule { Priority = priority, RegionTag = tag, Predicate = pred }); return this;
    //     }
    //     public PlacementTaggerBuilder AddSubregionRule(int priority, SubregionTag tag, Func<List<Vector2Int>, RegionTag, bool> pred) {
    //         _rules.Add(new TaggingRule { Priority = priority, SubregionTag = tag, Predicate = (cells, ctx) => ctx.HasValue && pred(cells, ctx.Value) }); return this;
    //     }
    //     public DungeonTagService Build() { return new DungeonTagService(_config, _rules); }
    // }
}
