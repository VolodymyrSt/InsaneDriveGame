using System;
using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace _Project.Code
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IGameStateMachine _stateMachine;

        private void OnValidate() =>
            _button ??= GetComponent<Button>();

        [Inject]
        private void Construct(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void Start()
        {
            _button.onClick.AddListener(() => {
                _stateMachine.Enter<LoadingLevelState>();
                _button.onClick.RemoveAllListeners();
            });
        }
    }
}