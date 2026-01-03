using System;
using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using _Project.Code.Core.Services.Audio;
using _Project.Code.Core.Services.EventService;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace _Project.Code
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IGameStateMachine _stateMachine;
        private IEventBus _eventBus;
        private IAudioService _audioService;

        private void OnValidate() =>
            _button ??= GetComponent<Button>();

        [Inject]
        private void Construct(IGameStateMachine stateMachine, IEventBus eventBus
        ,IAudioService audioService)
        {
            _stateMachine = stateMachine;
            _eventBus = eventBus;
            _audioService = audioService;
        }
        
        private void Start()
        {
            _eventBus.Subscribe(new CustomEvent<OnCustomTriggerTriggered>(x =>
            {
                Debug.Log("OnCustomTriggerTriggered");
            }));
            
            _button.onClick.AddListener(() => {
                _stateMachine.Enter<LoadingMenuState>();
                _button.onClick.RemoveAllListeners();
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _audioService.BuildSFX()
                    .WithCameraPosition()
                    .WithVolume(1f)
                    .Play("Shit");
        }
    }
}