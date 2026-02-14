using System;
using System.Collections.Generic;
using _Project.Code.GamePlay.Car.Parts.GearShifter;
using _Project.Code.GamePlay.Car.Parts.GearShifter.View;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Code.Configs.Car
{
    [CreateAssetMenu(fileName = "Gear Shift Node Config", menuName = "Configs/GearShiftNode")]
    public class GearShiftNodeSO : ScriptableObject
    {
        public GearMode Mode;
        public Vector2 TiltAngles;
        public List<NeighborDirection> Neighbors;

        public bool TryGetNeighborByDirection(Direction direction, out GearShiftNodeSO node)
        {
            node = Neighbors.Find(x => x.Direction == direction).Node;
            return node != null;
        }

        public Direction GetDirectionToNeighbor(GearMode mode)
        {
            var neighbor =  Neighbors.Find(x => x.Node.Mode == mode);
            
            if (neighbor.Node == null)
            {
                Debug.LogWarning($"[GearShiftNodeSO] '{name}' has no neighbor with mode {mode}");
                return Direction.None;
            }
            return neighbor.Direction;
        }
    }

    [Serializable]
    public struct NeighborDirection
    {
        public Direction Direction;
        public GearShiftNodeSO Node;
    }
}