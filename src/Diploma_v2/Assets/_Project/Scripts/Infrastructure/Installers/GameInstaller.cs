using _Project.Scripts.Infrastructure.StateMachines;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Windows {
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IWindowsCanvas>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
            Container.Bind<IWindowManager>().To<WindowManager>().AsSingle();
        }
    }
}