using UnityEngine;

namespace _Project.Code.GamePlay.Car.Parts.CarAxle
{
    public class FrontAxle : Axle
    {
        private float _currentYRotation;
        private float _targetYRotation;

        public FrontAxle(WheelCollider wheelRight, WheelCollider wheelLeft) : base(wheelRight, wheelLeft) { }

        public void PerformWheelsTurn(float requestedSteeringWheelTurn)
        {
            _targetYRotation = Mathf.Clamp(_currentYRotation + requestedSteeringWheelTurn * 5f, -30, 30);
            _targetYRotation += Mathf.Approximately(Mathf.Sign(_targetYRotation), 1) ? -0.3f : 0.3f;
            
            RotateWheels(_targetYRotation);
            
            WheelRight.steerAngle = _targetYRotation;
            WheelLeft.steerAngle = _targetYRotation;
            
            _currentYRotation = _targetYRotation;
        }
        
        private void RotateWheels(float targetYRotation)
        {
            WheelRight.transform.parent.localEulerAngles = new Vector3(WheelRight.transform.parent.localEulerAngles.x, targetYRotation, WheelRight.transform.parent.localEulerAngles.z);
            WheelLeft.transform.parent.localEulerAngles = new Vector3(WheelLeft.transform.parent.localEulerAngles.x, targetYRotation, WheelLeft.transform.parent.localEulerAngles.z);
        }
    }
}