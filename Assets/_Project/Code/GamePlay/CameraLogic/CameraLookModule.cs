using _Project.Code.Configs.Camera;
using UnityEngine;

namespace _Project.Code.GamePlay.CameraLogic
{
    public class CameraLookModule
    {
        private readonly Transform _cameraTransform; 
        private readonly CameraConfigSO _config; 
        
        private readonly float _sensitivity = 80f;  //Change to SaveLoadData
        private readonly float _lookResponse;
        
        private float _minPitch; 
        private float _maxPitch;
        
        private float _yaw;
        private float _pitch;
        
        private float _minYaw; 
        private float _maxYaw;
        
        private Vector2 _currentLook;
        
        private Transform _vehicle;
        private float _lastVehicleYaw;
        
        private CameraLookMode _mode;
        
        public CameraLookModule(Transform cameraTransform, CameraConfigSO config)
        {
            _cameraTransform = cameraTransform;
            _config = config;
            _lookResponse = config.LookResponse;
            
            InitializeYawPitch(cameraTransform);
            SetMode(CameraLookMode.Free);
        }

        public void UpdateLook(Vector3 inputLook) 
        { 
            if (_mode == CameraLookMode.Vehicle && _vehicle != null) 
                ApplyVehicleYaw();
            
            _currentLook = Vector2.Lerp(_currentLook, inputLook, 1f - Mathf.Exp(-_lookResponse * Time.deltaTime));
            
            _yaw += _currentLook.x * _sensitivity * Time.deltaTime;
            _pitch -= _currentLook.y * _sensitivity * Time.deltaTime;
            
            ApplyClamp();
            ApplyRotation();
        }

        public void SetMode(CameraLookMode mode) => 
            _mode = mode;
        
        public void SetVehicleLook(Transform vehicle, Vector3 forward, Vector2 pitchLimits, Vector2 yawLimits)
        {
            _vehicle = vehicle;

            _lastVehicleYaw = NormalizeAngle(vehicle.eulerAngles.y);
            
            var euler = Quaternion.LookRotation(forward).eulerAngles;

            _yaw = NormalizeAngle(euler.y);
            _pitch = NormalizeAngle(euler.x); 

            _minPitch = pitchLimits.x;
            _maxPitch = pitchLimits.y;
            _minYaw = yawLimits.x;
            _maxYaw = yawLimits.y;

            _mode = CameraLookMode.Vehicle;
        }
        
        private void ApplyRotation() => 
            _cameraTransform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        private void ApplyClamp()
        {
            if (_mode == CameraLookMode.Vehicle)
            {
                var vehicleYaw = NormalizeAngle(_vehicle.eulerAngles.y);
                var localYaw = Mathf.DeltaAngle(vehicleYaw, _yaw);

                localYaw = Mathf.Clamp(localYaw, _minYaw, _maxYaw);
                _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

                _yaw = vehicleYaw + localYaw;
            }
            else
                _pitch = Mathf.Clamp(_pitch, _config.MinPitch, _config.MaxPitch);
        }
        
        private void ApplyVehicleYaw()
        {
            var currentVehicleYaw = NormalizeAngle(_vehicle.eulerAngles.y);
            var deltaYaw = Mathf.DeltaAngle(_lastVehicleYaw, currentVehicleYaw);

            _yaw += deltaYaw;
            _lastVehicleYaw = currentVehicleYaw;
        }
        
        private void InitializeYawPitch(Transform cameraTransform)
        {
            var euler = cameraTransform.rotation.eulerAngles;
            _yaw = euler.y;
            _pitch = euler.x > 180 ? euler.x - 360 : euler.x;
        }
        
        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle > 180f) angle -= 360f;
            if (angle < -180f) angle += 360f;
            return angle;
        }
    }
}
