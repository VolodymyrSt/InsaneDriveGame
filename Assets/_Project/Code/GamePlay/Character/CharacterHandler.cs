using System;
using _Project.Code.Configs.Character;
using _Project.Code.Core.Services.Input;
using _Project.Code.Core.Services.StaticData;
using _Project.Code.GamePlay.Camera;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.GamePlay.Character
{
    public class CharacterHandler : MonoBehaviour, ICharacter
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _cameraHolder;
        
        private IInputService _inputService;
        private CharacterConfigSO _config;
        private CharacterMover _characterMover;
        private GravityApplier _gravityApplier;
        private JumpPerformer _jumpPerformer;
        
        private bool _isInitialized = false;

        public Transform CameraHolder => _cameraHolder;
        
        private void OnValidate() => 
            _characterController ??= GetComponent<CharacterController>();

        [Inject]
        private void Construct(IInputService inputService, IStaticDataService staticDataService)
        {
            _inputService = inputService;
            _config = staticDataService.CharacterConfig;
        }

        public void Init(ICamera cameraHandler)
        {
            _characterMover = new CharacterMover(_characterController, _inputService, cameraHandler, _config);
            _gravityApplier = new GravityApplier(_characterController);
            _jumpPerformer = new JumpPerformer(_characterController, _gravityApplier, _inputService, _config);
            
            _isInitialized = true;
        }
        
        private void Update()
        {
            if (!_isInitialized) return;
            
            _characterMover.Move();
            _gravityApplier.ApplyGravity();
            _jumpPerformer.PerformJump();
        }
    }
}