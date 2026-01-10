using System;
using _Project.Code.Configs.Character;
using _Project.Code.Core.Services.Input;
using _Project.Code.Core.Services.StaticData;
using _Project.Code.GamePlay.Camera;
using KinematicCharacterController;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.GamePlay.Character
{
    public class Character : MonoBehaviour, ICharacter
    {
        [Header("Base")]
        [SerializeField] private KinematicCharacterMotor _kinematicMotor;
        
        [Header("Camera Settings")]
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Transform _cameraTarget;
        
        private IInputService _input;
        private ICamera _camera;
        private CharacterConfigSO _config;
        private KinematicCharacterModule _kinematicCharacterModule;
        
        private bool _isInitialized = false;

        public Transform CameraHolder => _cameraHolder;
        public Transform CameraTarget => _cameraTarget;
        public Vector3 Acceleration => _kinematicCharacterModule.GetAcceleration();
        
        private void OnValidate() => 
            _kinematicMotor ??= GetComponent<KinematicCharacterMotor>();

        [Inject]
        private void Construct(IInputService inputService, IStaticDataService staticDataService)
        {
            _input = inputService;
            _config = staticDataService.CharacterConfig;
        }

        public void Init(ICamera cameraHandler)
        {
            _camera = cameraHandler;
            
            _kinematicCharacterModule = new KinematicCharacterModule(_kinematicMotor, _config, _cameraTarget);
            _kinematicCharacterModule.Init();
            _isInitialized = true;
        }
        
        private void Update()
        {
            if (!_isInitialized) return;
            
            _kinematicCharacterModule.UpdateInput(
                _input.GetPlayerMoveVector(), 
                _camera.Rotation, 
                _input.PlayerJumpHeld(), 
                _input.PlayerCrouchHeld(),
                _input.PlayerRunHeld()
            );
            
            _kinematicCharacterModule.UpdateBody();
        }
    }
}