using System;
using _Project.Code.Core.Services.Input;
using _Project.Code.Core.Services.StaticData;
using _Project.Code.GamePlay.Car;
using _Project.Code.GamePlay.Character;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace _Project.Code.GamePlay.CameraLogic
{
    public class CameraHandler : MonoBehaviour, ICamera
    {
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private Transform _springRoot;
        [SerializeField] private Transform _leanRoot;
        
        private IInputService _input;
        private IStaticDataService _staticDataService;
        private ICharacter _character;
        private CameraSpringModule _cameraSpringModule;
        private CameraLeanModule _cameraLeanModule;
        private CameraLookModule _cameraLookModule;
        
        private Transform _target;
        public Quaternion Rotation => transform.rotation;
        public Transform Transform => transform;
        public CinemachineCamera Camera => _camera;
        public CameraLookModule LookModule => _cameraLookModule;
        
        private bool _isInitialized = false;
        
        [Inject]
        private void Construct(IInputService inputService, IStaticDataService staticDataService)
        {
            _input = inputService;
            _staticDataService = staticDataService;
        }

        public void Init(ICharacter character)
        {
            _character = character;
            _target = character.Head;
            
            _cameraLookModule = new CameraLookModule(transform, _staticDataService.CameraConfig);
            _cameraSpringModule = new CameraSpringModule(_springRoot, _staticDataService.CameraConfig);
            _cameraLeanModule = new CameraLeanModule(_leanRoot, _staticDataService.CameraConfig);
            
            transform.SetParent(character.CameraHolder, false);
            
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;
            _cameraLookModule.UpdateLook(_input.GetPlayerLookVector());
        }

        private void LateUpdate()
        {
            if (!_isInitialized) return;
            UpdatePosition();
            _cameraSpringModule.UpdateSpring(_target.up);
            _cameraLeanModule.UpdateLean(_character.Acceleration, _target.up);
        }
        
        private void UpdatePosition() =>
            _camera.transform.position = _target.position;
    }
}