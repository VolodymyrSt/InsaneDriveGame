using _Project.Code.GamePlay.CameraLogic;
using KinematicCharacterController;
using UnityEngine;

namespace _Project.Code.GamePlay.Character
{
    public interface ICharacter
    {
        public Transform CameraHolder { get; }
        public Transform Head { get; }
        public Vector3 Acceleration { get; }
        public Transform Transform  { get; }
        public KinematicCharacterMotor Motor  { get; }

        void Init(ICamera cameraHandler);
        void ResetHeadPosition();
        void Activate();
        void Deactivate();
    }
}