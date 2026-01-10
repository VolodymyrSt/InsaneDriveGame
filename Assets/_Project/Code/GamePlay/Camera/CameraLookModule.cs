using _Project.Code.Configs.Camera;
using UnityEngine;

namespace _Project.Code.GamePlay.Camera
{
    public class CameraLookModule
    {
        private readonly Transform _cameraTransform; 
        
        private readonly float _sensitivity = 120f;  //Change to SaveLoadData
        
        private float _minPitch; 
        private float _maxPitch;
        
        private float _minYaw; 
        private float _maxYaw;
        
        private Vector3 _eulerAngles; 
        public CameraLookModule(Transform cameraTransform, CameraConfigSO config)
        {
            _cameraTransform = cameraTransform;

            ChangePitch(config.MinPitch, config.MaxPitch);
        }

        public void UpdateLook(Vector3 inputLook) 
        { 
            _eulerAngles.x += -inputLook.y * _sensitivity * Time.deltaTime;  // pitch
            _eulerAngles.y += inputLook.x * _sensitivity * Time.deltaTime;   // yaw
            
            _eulerAngles.x = Mathf.Clamp(_eulerAngles.x, _minPitch, _maxPitch);
            _cameraTransform.rotation = Quaternion.Euler(_eulerAngles);
        }

        public void ChangePitch(float minPitch, float maxPitch)
        {
            _minPitch = minPitch;
            _maxPitch = maxPitch;
        }
        
        public void ChangeYaw(float minYaw, float maxYaw)
        {
            _minYaw = minYaw;
            _maxYaw = maxYaw;
        }
    }
}