using _Project.Infrastructure.MVP.Core;
using Zenject;

namespace _Project.Infrastructure.SceneContexts.Scripts {
    public class MainMenuInstaller : MonoInstaller {
        public override void InstallBindings() =>
            WindowServiceInstaller.Install(Container);
    }
}