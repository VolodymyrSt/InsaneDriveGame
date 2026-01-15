using _Project.Code.Configs.Camera;
using UnityEngine;

namespace _Project.Code.GamePlay.CameraLogic
{
    public class CameraSpringModule
    {
        private readonly Transform _springRoot;
        
        private Vector3 _springPosition;
        private Vector3 _springVelocity;

        private readonly float _halfLife;
        private readonly float _frequency;
        private readonly float _angularDisplacement;
        private readonly float _linerDisplacement;
        
        public CameraSpringModule(Transform springRoot, CameraConfigSO config)
        {
            _springRoot = springRoot;

            _halfLife            = config.HalfLife;
            _frequency           = config.Frequency;
            _angularDisplacement = config.AngularDisplacement;
            _linerDisplacement   = config.LinerDisplacement;
            
            _springPosition = _springRoot.position;
            _springVelocity = Vector3.zero;
        }
        
        public void UpdateSpring(Vector3 up)
        {
            _springRoot.localPosition = Vector3.zero; 
            Spring(ref _springPosition, ref _springVelocity, _springRoot.position, _halfLife, _frequency, Time.deltaTime);
            
            var localSpringPosition = _springPosition -  _springRoot.position;
            var springHeight = Vector3.Dot(localSpringPosition, up);
            
            _springRoot.localEulerAngles = new Vector3(-springHeight * _angularDisplacement, 0f, 0f);
            _springRoot.localPosition = localSpringPosition * _linerDisplacement;
        }

        private void Spring(ref Vector3 current, ref Vector3 velocity, Vector3 target, float halfLife, float frequency, float timeStep)
        {
            var dampingRatio = -Mathf.Log(0.5f) / (frequency * halfLife);
            var f = 1.0f + 2.0f * timeStep * dampingRatio * frequency;
            var oo = frequency * frequency;
            var hoo = timeStep * oo;
            var hhoo = timeStep * hoo;
            var detInv = 1.0f / (f + hhoo);
            Vector2 detX = f * current + timeStep * velocity + hhoo * target;
            Vector2 detV = velocity + hoo * (target - current);
            current = detX * detInv;
            velocity = detV * detInv;
        }
    }
}