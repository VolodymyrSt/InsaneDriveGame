using UnityEngine;

namespace _Project.Code.GamePlay.Car.Parts.Engine
{
    public class Engine
    {
        private readonly Rigidbody _rigidbody;
        
        private readonly float _velocityLimit = 12f;
        private readonly float _accelerationStraight = 400f;
        
        private float _requestedGas;

        public Engine(Rigidbody rigidbody) => 
            _rigidbody = rigidbody;

        public void RequestInput(float requestedGas) =>
            _requestedGas = requestedGas;

        public void UpdateGear()
        {
            
        }
        
        public float GetMotorTorque()
        {
            var currentCarVelocity = _rigidbody.linearVelocity.magnitude;
            return _requestedGas * _accelerationStraight * (1.02f - currentCarVelocity / _velocityLimit);
        }
    }
}