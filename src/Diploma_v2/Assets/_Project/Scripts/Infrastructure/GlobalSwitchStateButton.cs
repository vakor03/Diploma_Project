using System;
using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GameplayStates;
using _Project.Scripts.Infrastructure.StateMachines.GlobalStates;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    [RequireComponent(typeof(Button))]
    public class GlobalSwitchStateButton : MonoBehaviour
    {
        private Button _button;

        [SerializeField] private NextState nextState;

        private GlobalStateMachine _globalStateMachine;

        [Inject]
        private void Construct(GlobalStateMachine globalStateMachine)
        {
            _globalStateMachine = globalStateMachine;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleClick);
        }

        private void HandleClick()
        {
switch (nextState)
            {
                case NextState.MainMenu:
                    _globalStateMachine.Enter<MainMenuState>();
                    break;
                case NextState.Gameplay:
                    _globalStateMachine.Enter<GameplayState>();
                    break;
                case NextState.Quit:
                    _globalStateMachine.Enter<QuitState>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum NextState
        {
            None = 0,
            MainMenu = 1,
            Gameplay = 2,
            Quit = 3,
        }
    }
}