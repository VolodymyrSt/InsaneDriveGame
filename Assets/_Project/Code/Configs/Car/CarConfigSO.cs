using System;
using System.Collections.Generic;
using _Project.Code.GamePlay.Car.Parts.GearShifter;
using UnityEngine;

namespace _Project.Code.Configs.Car
{
    [CreateAssetMenu(fileName = "Car Config", menuName = "Configs/Car")]
    public class CarConfigSO : ScriptableObject
    {
        [Header("Gears")]
        public List<GearData> Gears = new();
        
        [Header("Nodes")]
        public List<GearShiftNodeSO> Nodes = new();
        public GearShiftNodeSO StartingNode;

        private void OnValidate()
        {
            if (Gears.Count <= 0 || Gears == null) return;
            
            foreach (var gear in Gears)
                gear.Settings.Acceleration = Mathf.Clamp(gear.Settings.Acceleration, 0f, 1000f);
        }
    }
}