using System.Collections.Generic;
using System.IO;
using _Project.Features.MapGeneration.BSP;
using UnityEditor;
using UnityEngine;

namespace _Project.Features.MapGeneration.PNGExproter {
    public class DungeonToPNGExporter {
        public void Export(Dungeon dungeon, string filePath, int scale = 10, Dictionary<int, Color> colorMap = null, Color? borderColor = null) {
            int rows = dungeon.Matrix.Height;
            int cols = dungeon.Matrix.Width;

            if (colorMap == null) {
                colorMap = new Dictionary<int, Color> {
                    { 0, Color.black },
                    { 1, Color.white }
                };
            }

            Color bColor = borderColor ?? Color.magenta;
            Texture2D tex = new Texture2D(cols * scale, rows * scale, TextureFormat.RGB24, false) {
                filterMode = FilterMode.Point
            };

            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < cols; x++) {
                    int value = dungeon.Matrix[x, y];
                    Color c = colorMap.ContainsKey(value) ? colorMap[value] : Color.magenta;
                    bool inRoom = false;
                    foreach (var room in dungeon.Rooms) {
                        if (x >= room.x && x < room.xMax && y >= room.y && y < room.yMax) {
                            inRoom = true;
                            break;
                        }
                    }
                    for (int dy = 0; dy < scale; dy++) {
                        for (int dx = 0; dx < scale; dx++) {
                            int px = x * scale + dx;
                            int py = (rows - 1 - y) * scale + dy;
                            bool drawBorder = inRoom && (dx == 0 || dy == 0 || dx == scale - 1 || dy == scale - 1);
                            tex.SetPixel(px, py, drawBorder ? bColor : c);
                        }
                    }
                }
            }

            tex.Apply();
            byte[] bytes = tex.EncodeToPNG();
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllBytes(filePath, bytes);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            Application.OpenURL($"file://{filePath}");
            Debug.Log($"Dungeon PNG exported with room borders to: {filePath}");
        }
    }
}
