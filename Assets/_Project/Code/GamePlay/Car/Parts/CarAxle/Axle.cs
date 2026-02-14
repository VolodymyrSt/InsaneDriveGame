using UnityEngine;

namespace _Project.Code.GamePlay.Car.Parts.CarAxle
{
    public class Axle
    {
        protected readonly WheelCollider WheelRight;
        protected readonly WheelCollider WheelLeft;

        public Axle(WheelCollider wheelRight, WheelCollider wheelLeft)
        {
            WheelRight = wheelRight;
            WheelLeft = wheelLeft;
        }

        public void SetMotorTorque(float torque)
        {
            WheelRight.motorTorque = torque;
            WheelLeft.motorTorque = torque;
        }

        public void SetBreakTorque(float breakTorque)
        {
            WheelRight.brakeTorque = breakTorque;
            WheelLeft.brakeTorque = breakTorque;
        }

        public void Release() => 
            SetBreakTorque(0);
    }
}