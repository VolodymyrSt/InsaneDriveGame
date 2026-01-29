using _Project.Code.GamePlay.Car;
using _Project.Code.GamePlay.Character;
using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Code.GamePlay.CameraLogic
{
    public interface ICamera
    {
        Quaternion Rotation { get; }
        CinemachineCamera Camera { get; }
        Transform Transform { get; }
        CameraLookModule LookModule { get; }

        void Init(ICharacter target);
        void WithModifiers(bool withAnimation);
    }
}