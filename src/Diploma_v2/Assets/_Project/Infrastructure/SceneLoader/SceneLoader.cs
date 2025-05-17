using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.SceneLoader {
    public class SceneLoader : ISceneLoader {
        public void Load(string sceneName, bool loadAdditive = false, Action onComplete = null) =>
            Timing.RunCoroutine(LoadSceneCoroutine(sceneName, loadAdditive));

        private IEnumerator<float> LoadSceneCoroutine(string sceneName, bool loadAdditive, Action onComplete = null) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);

            while (!asyncLoad.isDone)
                yield return Timing.WaitForOneFrame;
            
            onComplete?.Invoke();
        }

        public void Unload(string sceneName) =>
            Timing.RunCoroutine(UnloadSceneCoroutine(sceneName));

        private IEnumerator<float> UnloadSceneCoroutine(string sceneName) {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            while (!asyncUnload.isDone)
                yield return Timing.WaitForOneFrame;
        }
    }
}