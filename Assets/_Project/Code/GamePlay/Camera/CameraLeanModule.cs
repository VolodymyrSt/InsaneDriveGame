using _Project.Code.Configs.Camera;
using UnityEngine;

namespace _Project.Code.GamePlay.Camera
{
    public class CameraLeanModule
    {
        private readonly Transform _leanRoot;
        
        private Vector3 _dampedAcceleration;
        private Vector3 _dampedAccelerationVelocity;
        
        private readonly float _attackDamping;
        private readonly float _decayDamping;
        private readonly float _strength;
        private readonly float _strengthResponse;
        
        private float _smoothStrength;
        
        public CameraLeanModule(Transform leanRoot, CameraConfigSO config)
        {
            _leanRoot = leanRoot;

            _attackDamping    = config.AttackDamping;
            _decayDamping     = config.DecayDamping;
            _strength         =  config.LeanStrength;
            _strengthResponse = config.StrengthResponse;
        }

        public void UpdateLean(Vector3 acceleration, Vector3 up)
        {
            var planarAcceleration = Vector3.ProjectOnPlane(acceleration, up);
            var damping = planarAcceleration.magnitude > _dampedAcceleration.magnitude 
                ? _attackDamping 
                : _decayDamping;
            
            _dampedAcceleration = Vector3.SmoothDamp(
                _dampedAcceleration,
                planarAcceleration,
                ref _dampedAccelerationVelocity,
                damping,
                float.PositiveInfinity,
                Time.deltaTime
            );
            
            var leanAxis =  Vector3.Cross(_dampedAcceleration.normalized, up).normalized;
            
            _leanRoot.localRotation = Quaternion.identity;

            _smoothStrength = Mathf.Lerp(_smoothStrength, _strength,
                1f - Mathf.Exp(-_strengthResponse * Time.deltaTime));
            
            _leanRoot.rotation = Quaternion.AngleAxis(_dampedAcceleration.magnitude * _smoothStrength, leanAxis) * _leanRoot.rotation;
        }
    }
}