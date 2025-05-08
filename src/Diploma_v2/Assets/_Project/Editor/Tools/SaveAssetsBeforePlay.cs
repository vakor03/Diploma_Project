using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace _Project.Editor.Tools {

    [InitializeOnLoad]
    public static class SaveAssetsBeforePlay {
        private static bool _playAfterCompile;

        static SaveAssetsBeforePlay() {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            CompilationPipeline.compilationFinished += OnCompilationFinished;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingEditMode) {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                _playAfterCompile = true;

                EditorApplication.isPlaying = false;
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

                CompilationPipeline.RequestScriptCompilation();
            }
        }

        private static void OnCompilationFinished(object context) {
            if (!_playAfterCompile)
                return;
            _playAfterCompile = false;

            EditorApplication.isPlaying = true;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
    }
}