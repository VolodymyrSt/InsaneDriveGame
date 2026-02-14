using System;
using UnityEngine;

namespace _Project.Code.GamePlay.Car.Parts.GearShifter
{
    [Serializable]
    public struct GearData
    {
        public GearMode mode;
        public GearSettings Settings;
    }

    [Serializable]
    public class GearSettings
    {
        [Range(0f, 100f)] public float VelocityLimit = 0;
        public float Acceleration;
    }
}