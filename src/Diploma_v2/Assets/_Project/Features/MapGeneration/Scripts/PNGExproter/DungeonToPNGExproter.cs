using System.Collections.Generic;
using System.IO;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Tagging;
using UnityEditor;
using UnityEngine;

namespace _Project.Features.MapGeneration.PNGExproter {
    public class DungeonToPNGExporter {
        private Texture2D _texture;
        private int _scale;
        private Color _borderColor;
        private int _rowCount;
        private int _colCount;
        public void Export(Dungeon dungeon, string filePath, int scale1 = 10, Dictionary<int, Color> colorMap = null, Color? borderColor = null) {
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

            DrawOverallMatrix(dungeon, colorMap);

            DrawRooms(dungeon);
            DrawTunnels(dungeon);
            DrawTagRules(dungeon);

            _texture.Apply();
            byte[] bytes = _texture.EncodeToPNG();
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllBytes(filePath, bytes);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            Application.OpenURL($"file://{filePath}");
            Debug.Log($"Dungeon PNG exported with room borders to: {filePath}");
        }

        private void DrawTagRules(Dungeon dungeon) {
            foreach ((SubSpaceTag tag, List<Vector2Int> value) in dungeon.Tags.TaggedSubSpaces) {
                foreach (Vector2Int cell in value)
                    DrawCell(cell, true, Color.yellow);
            }
        }

        private void DrawOverallMatrix(Dungeon dungeon, Dictionary<int, Color> colorMap) {
            for (int y = 0; y < _rowCount; y++)
            for (int x = 0; x < _colCount; x++) {
                int value = dungeon.Matrix[x, y];
                Color c = colorMap.ContainsKey(value) ? colorMap[value] : Color.magenta;
                bool useBorder = false;
                foreach (var room in dungeon.Rooms) {
                    if (x >= room.PartitionBounds.x && x < room.PartitionBounds.xMax && y >= room.PartitionBounds.y && y < room.PartitionBounds.yMax) {
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
                    int py = (_rowCount - 1 - y) * _scale + dy;
                    bool drawBorder = withBorder && (dx == 0 || dy == 0 || dx == _scale - 1 || dy == _scale - 1);
                    _texture.SetPixel(px, py, drawBorder ? _borderColor : c);
                }
            }
        }

        private void DrawCell(Vector2Int position, bool withBorder, Color c) =>
            DrawCell(position.x, position.y, withBorder, c);
    }
}
