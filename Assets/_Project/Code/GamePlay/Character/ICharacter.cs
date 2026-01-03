using _Project.Code.GamePlay.Camera;
using UnityEngine;

namespace _Project.Code.GamePlay.Character
{
    public interface ICharacter
    {
        public Transform CameraHolder { get; }

        void Init(ICamera cameraHandler);
    }
}