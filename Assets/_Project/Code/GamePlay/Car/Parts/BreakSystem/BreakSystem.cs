using _Project.Code.GamePlay.Car.Parts.CarAxle;

namespace _Project.Code.GamePlay.Car.Parts.BreakSystem
{
    public class BreakSystem
    {
        private readonly float _deceleration = 400f;
        
        private readonly Axle _frontAxle;
        private readonly Axle _rearAxle;
        private readonly DrivetrainType _breakDrivetrainType;
        
        public BreakSystem(Axle front, Axle rear, DrivetrainType breakType)
        {
            _frontAxle = front;
            _rearAxle = rear;
        }

        public void Break()
        {
            var breakTorque = _deceleration * 10;

            switch (_breakDrivetrainType)
            {
                case DrivetrainType.FWD:
                    _frontAxle.SetBreakTorque(breakTorque);
                    break;
                case DrivetrainType.RWD:
                    _rearAxle.SetBreakTorque(breakTorque);
                    break;
                default:
                    _frontAxle.SetBreakTorque(breakTorque);
                    _rearAxle.SetBreakTorque(breakTorque);
                    break;
            }
        }

        public void Release()
        {
            switch (_breakDrivetrainType)
            {
                case DrivetrainType.FWD:
                    _frontAxle.Release();
                    break;
                case DrivetrainType.RWD:
                    _rearAxle.Release();
                    break;
                default:
                    _frontAxle.Release();
                    _rearAxle.Release();
                    break;
            }
        }
    }
}