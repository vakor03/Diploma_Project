using _Project.Features.MapGeneration.Tagging;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Features.MapGeneration.PNGExproter {
    [CreateAssetMenu(fileName = "TagVisualizationConfig", menuName = "Dungeon/Tag Visualization Config")]
    public class TagVisualizationConfig : ScriptableObject
    {
        [FoldoutGroup("Global Place Tag Colors")]
        public SerializedDictionary<GlobalPlaceTag, Color> GlobalPlaceTagColors = new()
        {
            { GlobalPlaceTag.None, Color.gray },
            { GlobalPlaceTag.InitialRoom, Color.green },
            { GlobalPlaceTag.TreasureRoom, Color.yellow },
            { GlobalPlaceTag.BossRoom, Color.red },
            { GlobalPlaceTag.MainCorridor, Color.blue },
            { GlobalPlaceTag.SecretPassage, Color.magenta }
        };
    
        [FoldoutGroup("Macro Tag Colors")]
        public SerializedDictionary<MacroTag, Color> MacroTagColors = new()
        {
            { MacroTag.None, Color.gray },
            { MacroTag.Floor, new Color(0.6f, 0.4f, 0.2f)}, // Brown
            { MacroTag.Wall, Color.black },
            { MacroTag.Ceiling, Color.cyan },
            { MacroTag.Door, new Color(0.5f, 0.25f, 0f)}, // Dark brown
            { MacroTag.Entrance, Color.green },
            { MacroTag.Exit, Color.red }
        };
    
        [FoldoutGroup("Micro Tag Colors")]
        public SerializedDictionary<MicroTag, Color> MicroTagColors = new()
        {
            { MicroTag.None, Color.gray },
            { MicroTag.PlayerSpawnPoint, Color.green },
            { MicroTag.EnemySpawnPoint, Color.red },
            { MicroTag.ChestSpawnPoint, Color.yellow },
            { MicroTag.TorchPosition, new Color(1f, 0.6f, 0f)}
        };
    
        [FoldoutGroup("Visualization Settings")]
        public enum TagLayerPriority
        {
            MicroTags,
            MacroTags,
            GlobalPlaceTags
        }
    
        [FoldoutGroup("Visualization Settings")]
        public TagLayerPriority VisualizationPriority = TagLayerPriority.MicroTags;
    
        [FoldoutGroup("Visualization Settings")]
        [Range(0.1f, 1f)]
        public float ColorBlendFactor = 0.7f; // For blending multiple tag colors
    
        [FoldoutGroup("Visualization Settings")]
        public bool ShowRoomBoundaries = true;
    
        [FoldoutGroup("Visualization Settings")]
        public Color RoomBoundaryColor = Color.white;
    
        [FoldoutGroup("Visualization Settings")]
        [Range(1, 5)]
        public int BoundaryThickness = 1;
    }
}