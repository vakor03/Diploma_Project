using System;
using _Project.Scripts.Infrastructure.StateMachines;
using _Project.Scripts.Infrastructure.StateMachines.GameplayStates;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    [RequireComponent(typeof(Button))]
    public class GameplaySwitchStateButton : MonoBehaviour
    {
        private Button _button;

        [SerializeField] private NextState nextState;

        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
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
                case NextState.GameOver:
                    _gameplayStateMachine.Enter<GameOverState>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum NextState
        {
            None = 0,
            Game = 1,
            GameOver = 2,
        }
    }
}