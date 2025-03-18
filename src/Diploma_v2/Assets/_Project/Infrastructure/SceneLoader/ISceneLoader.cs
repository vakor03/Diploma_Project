namespace _Project.Scripts.Infrastructure.SceneLoader
{
    public interface ISceneLoader
    {
        public void Load(string sceneName, bool loadAdditive = false);
        void Unload(string sceneName);
    }
}