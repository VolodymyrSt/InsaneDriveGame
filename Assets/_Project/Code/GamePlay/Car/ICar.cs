using UnityEngine;

namespace _Project.Code.GamePlay.Car
{
    public interface ICar
    {
        Transform Transform { get; }
        void Init();
    }
}