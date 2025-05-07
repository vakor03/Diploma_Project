using System.Collections.Generic;
using System.IO;
using _Project.Features.Installers;
using _Project.Features.MapGeneration.BSP;
using _Project.Features.MapGeneration.Matrix;
using _Project.Scripts.Infrastructure.AssetProviders;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Project.Features.MapGeneration.PNGExproter {
    public class DungeonExporterWindow : EditorWindow {
        [Inject] private IDungeonGeneratorService _dungeonGeneratorService;
        [Inject] private DungeonToPNGExporter _pngExproter;

        private int _scale = 10;
        private string _fileName = "Dungeon.png";
        private DungeonGenerationConfiguration _config;

        List<MatrixValueColor> valueColors = new List<MatrixValueColor>
            { new MatrixValueColor { value = 0, color = Color.black }, new MatrixValueColor { value = 1, color = Color.white } };

        [MenuItem("Tools/Matrix PNG Exporter")]
        public static void ShowWindow() {
            DungeonExporterWindow window = GetWindow<DungeonExporterWindow>("Matrix PNG Exporter");
            DiContainer container = ConstructContainerForCurrentWindow();
            container.Inject(window);
            window.Show();
        }
        
        private void OnEnable()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        private void OnAfterAssemblyReload() {
            DiContainer container = ConstructContainerForCurrentWindow();
            container.Inject(this);
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }

        private static DiContainer ConstructContainerForCurrentWindow() {
            DiContainer container = new DiContainer();
            container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            container.BindInterfacesTo<StaticDataService>().AsSingle();
            SeedServiceInstaller.Install(container);
            MatrixInstaller.Install(container);
            DungeonGeneratorInstaller.Install(container);
            DungeonExporterEditorInstaller.Install(container);

            foreach (IInitializable initializable in container.ResolveAll<IInitializable>())
                initializable.Initialize();
            return container;
        }

        private void OnGUI() {
            _scale = EditorGUILayout.IntField("Scale", _scale);
            _fileName = EditorGUILayout.TextField("File Name", _fileName);
            _config = EditorGUILayout.ObjectField("Configuration", _config, typeof(DungeonGenerationConfiguration), false) as DungeonGenerationConfiguration;
            
            EditorGUILayout.LabelField("Value Colors");
            for (int i = 0; i < valueColors.Count; i++) {
                var entry = valueColors[i];
                EditorGUILayout.BeginHorizontal();
                entry.value = EditorGUILayout.IntField(entry.value);
                entry.color = EditorGUILayout.ColorField(entry.color);
                EditorGUILayout.EndHorizontal();
                valueColors[i] = entry;
            }

            if (GUILayout.Button("Generate & Export PNG"))
                GenerateAndExport();
        }

        private void GenerateAndExport() {
            Dungeon dungeon = _dungeonGeneratorService.GenerateDungeon(_config);
            Dictionary<int, Color> map = new Dictionary<int, Color>();
            foreach (var vc in valueColors)
                map[vc.value] = vc.color;
            string path = Path.Combine(Application.dataPath, _fileName);
            _pngExproter.Export(dungeon, path, _scale, map);
        }
    }
}