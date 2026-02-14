using System;
using _Project.Code.Configs.Character;
using _Project.Code.Core.Services.EventService;
using _Project.Code.Core.Services.Input;
using _Project.Code.Core.Services.StaticData;
using _Project.Code.GamePlay.CameraLogic;
using _Project.Code.GamePlay.Car.Parts.GearShifter;
using _Project.Code.Test;
using KinematicCharacterController;
using UnityEngine;
using VContainer;

namespace _Project.Code.GamePlay.Character
{
    public class Character : MonoBehaviour, ICharacter
    {
        [Header("Base")]
        [SerializeField] private KinematicCharacterMotor _kinematicMotor;
        
        [Header("Camera Settings")]
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Transform _head;
        
        [Header("Interaction Settings")]
        [SerializeField] private LayerMask _interactionLayer;
        
        private IInputService _input;
        private ICamera _camera;
        private IEventBus _eventBus;
        private CharacterConfigSO _config;
        private KinematicCharacterModule _kinematicCharacterModule;
        private CharacterInteractorModule _characterInteractorModule;
        
        private bool _isInitialized = false;
        private bool _canProcessMovement = true;
        private Vector3 _baseHeadPosition;

        public Transform CameraHolder => _cameraHolder;
        public Transform Head => _head;
        public Transform Transform => _kinematicMotor.Transform;
        public KinematicCharacterMotor Motor => _kinematicMotor;
        public Vector3 Acceleration => _kinematicCharacterModule.GetAcceleration();
        
        private void OnValidate() => 
            _kinematicMotor ??= GetComponent<KinematicCharacterMotor>();

        [Inject]
        private void Construct(IInputService inputService, IStaticDataService staticDataService
            , IEventBus eventBus)
        {
            _input = inputService;
            _config = staticDataService.CharacterConfig;
            _eventBus  = eventBus;
        }

        public void Init(ICamera cameraHandler)
        {
            _camera = cameraHandler;
            _baseHeadPosition = _head.localPosition;
            
            _kinematicCharacterModule = new KinematicCharacterModule(_kinematicMotor, _config, _head);
            _characterInteractorModule = new CharacterInteractorModule(_camera, _head, _config, _interactionLayer, _eventBus);
            _kinematicCharacterModule.Init();
            
            _input.OnPlayerInteracted += OnInteracted;

            Activate();
            _isInitialized = true;
        }
        
        private void Update()
        {
            if (!_isInitialized) return;
            if (_canProcessMovement)
                UpdateKinematicMovement();
                
            _characterInteractorModule.RequestInput(_input.IsMousePressed);
            _characterInteractorModule.UpdateTarget();
            _characterInteractorModule.TryGrabSmth();
        }
        
        public void Activate()
        {
            _canProcessMovement = true;
            _kinematicMotor.enabled = true;
            _kinematicMotor.Capsule.enabled = true;
        }

        public void Deactivate()
        {
            _canProcessMovement = false;
            _kinematicMotor.enabled = false;
            _kinematicMotor.Capsule.enabled = false;
        }
        
        public void ResetHeadPosition() =>
            _head.localPosition = _baseHeadPosition;

        private void OnInteracted() => 
            _characterInteractorModule.TryInteract();
        
        private void UpdateKinematicMovement()
        {
            _kinematicCharacterModule.RequestInput(
                _input.GetCharacterMoveVector(), 
                _camera.Rotation, 
                _input.PlayerJumpHeld(), 
                _input.PlayerCrouchHeld(),
                _input.PlayerSprintHeld()
            );

            _kinematicCharacterModule.UpdateBody();
        }
        
        private void OnDestroy()
        {
            if (_input != null)
                _input.OnPlayerInteracted -= OnInteracted;
        }
    }
}