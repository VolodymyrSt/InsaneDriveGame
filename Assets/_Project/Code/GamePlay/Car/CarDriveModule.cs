using UnityEngine;

namespace _Project.Code.GamePlay.Car
{
    public class CarDriveModule
    {
        private readonly WheelCollider _wheelFR;
        private readonly WheelCollider _wheelFL;
        private readonly WheelCollider _wheelRR;
        private readonly WheelCollider _wheelRL;
        private readonly Rigidbody _rigidBody;
        
        private float _velocityLimit = 12f;
        private float _accelerationStraight = 400f;
        
        private float _currentYRotation;
        private float _targetYRotation;
        
        private Vector2 _requestedMovement;

        public CarDriveModule(WheelCollider wheelFr, WheelCollider wheelFl, WheelCollider wheelRr, WheelCollider wheelRl,
            Rigidbody rigidBody)
        {
            _wheelFR = wheelFr;
            _wheelFL = wheelFl;
            _wheelRR = wheelRr;
            _wheelRL = wheelRl;
            _rigidBody = rigidBody;
        }

        public void Init()
        {
          
        }

        public void RequestInput(Vector2 requestedMovement)
        {
            _requestedMovement = requestedMovement;
        }

        public void Drive()
        {
            var currentVelocity = _rigidBody.linearVelocity.magnitude;
            
            _targetYRotation = Mathf.Clamp(_currentYRotation + _requestedMovement.x * 5f, -30, 30);
            _targetYRotation += Mathf.Sign(_targetYRotation) == 1 ? -0.3f : 0.3f;
            
            RotateWheels(_targetYRotation);
            
            _wheelFR.steerAngle = _targetYRotation;
            _wheelFL.steerAngle = _targetYRotation;
            
            _currentYRotation = _targetYRotation;
            
            _wheelFL.motorTorque = _requestedMovement.y * _accelerationStraight * (1.02f - currentVelocity / _velocityLimit);
            _wheelFR.motorTorque = _requestedMovement.y * _accelerationStraight * (1.02f - currentVelocity / _velocityLimit);
        }

        private void RotateWheels(float targetYRotation)
        {
            _wheelFL.transform.parent.localEulerAngles = new Vector3(_wheelFL.transform.parent.localEulerAngles.x, targetYRotation, _wheelFL.transform.parent.localEulerAngles.z);
            _wheelFR.transform.parent.localEulerAngles = new Vector3(_wheelFR.transform.parent.localEulerAngles.x, targetYRotation, _wheelFR.transform.parent.localEulerAngles.z);
        }
    }
}