using System.Linq;
using _Project.Code.GamePlay.Car.Parts.GearShifter;
using UnityEngine;

namespace _Project.Code.Test
{ 
    public class GearShifterHandleTest : MonoBehaviour
    {
        [System.Serializable]
        public class GearPose
        {
            public GearMode mode;
            public Vector3 eulerRotation;
        }

        [SerializeField] private GearPose[] _gearPoses;
        [SerializeField] private float _snapSpeed = 10f;

        public GearMode CurrentGear { get; private set; }
        public void SetGear(GearMode mode)
        {
            CurrentGear = mode;

            var pose = _gearPoses.First(p => p.mode == mode);

            transform.rotation = Quaternion.Euler(pose.eulerRotation);
        }
        
        
        public void SnapToNearestGear(GearShifterHandleTest shifter)
        {
            float bestAngle = float.MaxValue;
            GearMode bestGear = GearMode.Neutral;

            foreach (var pose in _gearPoses)
            {
                float angle = Quaternion.Angle(
                    shifter.transform.rotation,
                    Quaternion.Euler(pose.eulerRotation));

                if (angle < bestAngle)
                {
                    bestAngle = angle;
                    bestGear = pose.mode;
                }
            }

            shifter.SetGear(bestGear);
        }
    }
}