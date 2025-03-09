using UnityEngine;

namespace _Project.Scripts.Infrastructure.StateMachines.GlobalStates
{
    public class QuitState : IState
    {
        public void Enter()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public void Exit()
        {
        }
    }
}