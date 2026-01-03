using System;
using UnityEngine;

namespace _Project.Code.Core.Factory.Pool
{
    [Serializable]
    public abstract class PoolableData { }

    [Serializable]
    public class WarpPoolableData : PoolableData
    {
        public Transform Parent;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}