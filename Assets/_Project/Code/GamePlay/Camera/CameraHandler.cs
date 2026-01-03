using System;
using _Project.Code.Core.Services.Input;
using _Project.Code.GamePlay.Character;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace _Project.Code.GamePlay.Camera
{
    public class CameraHandler : MonoBehaviour, ICamera
    {
        [SerializeField] private float _sensitivity = 120f;
        [SerializeField] private float _minPitch = -60f;
        [SerializeField] private float _maxPitch = 60f;
        [SerializeField] private CinemachineCamera _camera;
        
        private IInputService _inputService;
        private CinemachinePanTilt _panTilt;
        
        [Inject]
        private void Construct(IInputService inputService) =>
            _inputService = inputService;

        public void Init(Transform target)
        {
            _panTilt = _camera.GetComponent<CinemachinePanTilt>();
            
            _camera.Target = new CameraTarget {
                TrackingTarget = target
            };
        }
        
        public Vector3 GetNormalizedForward()
        {
            var forward = transform.forward;
            forward.y = 0;
            return forward.normalized;
        }

        public Vector3 GetNormalizedRight()
        {
            var right = transform.right;
            right.y = 0;
            return right.normalized;
        }

        private void Update() => PerformRotation();

        private void PerformRotation()
        {
            var look = _inputService.GetPlayerLookVector();
            
            _panTilt.PanAxis.Value += look.x * _sensitivity * Time.deltaTime;
            _panTilt.TiltAxis.Value -= look.y * _sensitivity * Time.deltaTime;

            _panTilt.TiltAxis.Value = Mathf.Clamp(_panTilt.TiltAxis.Value, _minPitch, _maxPitch);
        }
    }
}