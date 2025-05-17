using System;

namespace _Project.Scripts.Infrastructure.SceneLoader
{
    public interface ISceneLoader
    {
        public void Load(string sceneName, bool loadAdditive = false, Action onComplete = null);
        void Unload(string sceneName);
    }
}