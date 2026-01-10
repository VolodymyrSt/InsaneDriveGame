using _Project.Code.GamePlay.Camera;
using UnityEngine;

namespace _Project.Code.GamePlay.Character
{
    public interface ICharacter
    {
        public Transform CameraHolder { get; }
        public Transform CameraTarget { get; }
        public Vector3 Acceleration { get; }

        void Init(ICamera cameraHandler);
    }
}