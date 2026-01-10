using System;
using _Project.Code.Core.Services.Input;
using _Project.Code.Core.Services.StaticData;
using _Project.Code.GamePlay.Character;
using UnityEngine;
using VContainer;

namespace _Project.Code.GamePlay.Camera
{
    public class CameraHandler : MonoBehaviour, ICamera
    {
        [SerializeField] private Transform _camera;
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
        
        [Inject]
        private void Construct(IInputService inputService, IStaticDataService staticDataService)
        {
            _input = inputService;
            _staticDataService = staticDataService;
        }

        public void Init(ICharacter character)
        {
            _character = character;
            _target = character.CameraTarget;
            
            _cameraLookModule = new CameraLookModule(transform, _staticDataService.CameraConfig);
            _cameraSpringModule = new CameraSpringModule(_springRoot, _staticDataService.CameraConfig);
            _cameraLeanModule = new CameraLeanModule(_leanRoot, _staticDataService.CameraConfig);
            
            transform.SetParent(character.CameraHolder, false);
        }
        
        private void LateUpdate()
        {
            UpdatePosition();
            _cameraLookModule.UpdateLook(_input.GetPlayerLookVector());
            _cameraSpringModule.UpdateSpring(_target.up);
            _cameraLeanModule.UpdateLean(_character.Acceleration, _target.up);
        }
        
        private void UpdatePosition() =>
            _camera.position = _target.position;
    }
}