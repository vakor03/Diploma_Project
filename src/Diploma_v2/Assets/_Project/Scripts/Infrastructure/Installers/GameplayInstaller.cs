using _Project.Scripts.Infrastructure.StateMachines;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers {
    public class GameplayInstaller : MonoInstaller {
        public override void InstallBindings() {
            BindStatesFactory();

            BindGameplayStateMachine();
        }

        private void BindGameplayStateMachine() {
            Container.Bind<GameplayStateMachine>().AsSingle();
        }

        private void BindStatesFactory() {
            Container.Bind<StatesFactory>().AsSingle();
        }
    }
}