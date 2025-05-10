using System.Collections.Generic;
using System.IO;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Tagging;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEditor;
using UnityEngine;

namespace _Project.Features.MapGeneration.PNGExproter {
    public class DungeonToPNGExporter {
        private Texture2D _texture;
        private int _scale;
        private Color _borderColor;
        private int _rowCount;
        private int _colCount;

        private TagVisualizationConfig _tagVisualizationConfig;

        public DungeonToPNGExporter(IStaticDataService staticDataService) =>
            _tagVisualizationConfig = staticDataService.GetTagVisualizationConfig();

        public void Export(Dungeon dungeon, string filePath, int scale1 = 10, Dictionary<int, Color> colorMap = null,
                           Color? borderColor = null) {
            _rowCount = dungeon.Matrix.Height;
            _colCount = dungeon.Matrix.Width;

            _scale = scale1;

            if (colorMap == null) {
                colorMap = new Dictionary<int, Color> {
                    { 0, Color.black },
                    { 1, Color.white }
                };
            }

            _borderColor = borderColor ?? Color.magenta;
            _texture = new Texture2D(_colCount * _scale, _rowCount * _scale, TextureFormat.RGB24, false) {
                filterMode = FilterMode.Point
            };

            // DrawOverallMatrix(dungeon, colorMap);
            //
            // DrawRooms(dungeon);
            // DrawTunnels(dungeon);
            DrawTagRules(dungeon, _tagVisualizationConfig);

            _texture.Apply();
            byte[] bytes = _texture.EncodeToPNG();
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllBytes(filePath, bytes);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            Application.OpenURL($"file://{filePath}");
            Debug.Log($"Dungeon PNG exported with room borders to: {filePath}");
        }

        private void DrawTagRules(Dungeon dungeon, TagVisualizationConfig config)
        {
            // Using byte mask approach
            if ((config.VisualizationPriority & TagVisualizationConfig.TagLayerPriority.GlobalPlaceTags) != 0)
            {
                DrawGlobalPlaceTags(dungeon, config);
            }
    
            if ((config.VisualizationPriority & TagVisualizationConfig.TagLayerPriority.MacroTags) != 0)
            {
                DrawMacroTags(dungeon, config);
            }
    
            if ((config.VisualizationPriority & TagVisualizationConfig.TagLayerPriority.MicroTags) != 0)
            {
                DrawMicroTags(dungeon, config);
            }
    
            if ((config.VisualizationPriority & TagVisualizationConfig.TagLayerPriority.RoomBoundaries) != 0)
            {
                DrawRoomBoundaries(dungeon, config);
            }
        }
        
        private void DrawRoomBoundaries(Dungeon dungeon, TagVisualizationConfig config)
        {
            // Draw room outline using a different approach to make it visible
            foreach (Room room in dungeon.Rooms)
            {
                HashSet<Vector2Int> roomCells = new HashSet<Vector2Int>(room.Cells);
        
                foreach (Vector2Int cell in room.Cells)
                {
                    // Check if this cell is on the boundary
                    bool isBoundary = false;
            
                    // Check all four directions
                    Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            
                    foreach (Vector2Int direction in directions)
                    {
                        Vector2Int adjacentCell = cell + direction;
                
                        // If any adjacent cell is not part of the room, this is a boundary cell
                        if (!roomCells.Contains(adjacentCell))
                        {
                            isBoundary = true;
                            break;
                        }
                    }
            
                    if (isBoundary)
                    {
                        // Draw boundary with specified thickness
                        for (int i = 0; i < config.BoundaryThickness; i++)
                        {
                            DrawCell(cell, true, config.RoomBoundaryColor);
                        }
                    }
                }
            }
        }

        private void DrawGlobalPlaceTags(Dungeon dungeon, TagVisualizationConfig config) {
            // Draw room global place tags
            foreach (Room room in dungeon.Rooms) {
                GlobalPlaceTag tag = dungeon.Tags.GetGlobalPlaceTagForRoom(room);
                if (tag != GlobalPlaceTag.None && config.GlobalPlaceTagColors.TryGetValue(tag, out Color color)) {
                    // Make the color semi-transparent for better layering
                    color.a = config.ColorBlendFactor;

                    foreach (Vector2Int cell in room.Cells) {
                        DrawCell(cell, true, color);
                    }
                }
            }

            // Draw tunnel global place tags
            foreach (Tunnel tunnel in dungeon.Tunnels) {
                GlobalPlaceTag tag = dungeon.Tags.GetGlobalPlaceTagForTunnel(tunnel);
                if (tag != GlobalPlaceTag.None && config.GlobalPlaceTagColors.TryGetValue(tag, out Color color)) {
                    color.a = config.ColorBlendFactor;

                    foreach (Vector2Int cell in tunnel.Cells) {
                        DrawCell(cell, true, color);
                    }
                }
            }
        }

        private void DrawMacroTags(Dungeon dungeon, TagVisualizationConfig config) {
            // Get all positions with macro tags
            foreach (KeyValuePair<Vector2Int, MacroTag> taggedPosition in dungeon.Tags.GetAllMacroTaggedPositions()) {
                if (taggedPosition.Value != MacroTag.None && config.MacroTagColors.TryGetValue(taggedPosition.Value, out Color color)) {
                    // If we're in macro tag priority mode, use full opacity
                    // Otherwise, blend with other tags
                    if (config.VisualizationPriority == TagVisualizationConfig.TagLayerPriority.MacroTags) {
                        DrawCell(taggedPosition.Key, true, color);
                    }
                    else {
                        color.a = config.ColorBlendFactor;
                        DrawCell(taggedPosition.Key, true, color);
                    }
                }
            }
        }

        private void DrawMicroTags(Dungeon dungeon, TagVisualizationConfig config) {
            // Get all positions with micro tags
            foreach (KeyValuePair<Vector2Int, MicroTag> taggedPosition in dungeon.Tags.GetAllMicroTaggedPositions()) {
                if (taggedPosition.Value != MicroTag.None && config.MicroTagColors.TryGetValue(taggedPosition.Value, out Color color)) {
                    DrawCell(taggedPosition.Key, true, color);
                }
            }
        }

        private void DrawOverallMatrix(Dungeon dungeon, Dictionary<BlockType, Color> colorMap) {
            for (int y = 0; y < _rowCount; y++)
            for (int x = 0; x < _colCount; x++) {
                BlockType value = dungeon.Matrix[x, y];
                Color c = colorMap.ContainsKey(value) ? colorMap[value] : Color.magenta;
                bool useBorder = false;
                foreach (var room in dungeon.Rooms) {
                    if (x >= room.PartitionBounds.x && x < room.PartitionBounds.xMax && y >= room.PartitionBounds.y &&
                        y < room.PartitionBounds.yMax) {
                        useBorder = true;
                        break;
                    }
                }

                DrawCell(x, y, useBorder, c);
            }
        }

        private void DrawRooms(Dungeon dungeon) {
            foreach (Room room in dungeon.Rooms)
            foreach (Vector2Int cell in room.Cells)
                DrawCell(cell, true, Color.green);
        }

        private void DrawTunnels(Dungeon dungeon) {
            foreach (Tunnel dungeonTunnel in dungeon.Tunnels) {
                foreach (Vector2Int cell in dungeonTunnel.Cells)
                    DrawCell(cell, true, Color.blue);
                DrawCell(dungeonTunnel.End, true, Color.red);
                DrawCell(dungeonTunnel.Start, true, Color.red);
            }
        }

        private void DrawCell(int x, int y, bool withBorder, Color c) {
            for (int dy = 0; dy < _scale; dy++) {
                for (int dx = 0; dx < _scale; dx++) {
                    int px = x * _scale + dx;
                    int py = y * _scale + dy;  // Fixed: removed the mirroring calculation
                    bool drawBorder = withBorder && (dx == 0 || dy == 0 || dx == _scale - 1 || dy == _scale - 1);
                    _texture.SetPixel(px, py, drawBorder ? _borderColor : c);
                }
            }
        }

        private void DrawCell(Vector2Int position, bool withBorder, Color c) =>
            DrawCell(position.x, position.y, withBorder, c);
    }
}