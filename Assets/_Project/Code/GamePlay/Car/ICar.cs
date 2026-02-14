using _Project.Code.GamePlay.CameraLogic;
using UnityEngine;

namespace _Project.Code.GamePlay.Car
{
    public interface ICar
    {
        Transform Transform { get; }
        void Init(ICamera camera);

        void SetCarIgnited(bool isCarIgnited);
        void SetInCar(bool isInCar);
    }
}