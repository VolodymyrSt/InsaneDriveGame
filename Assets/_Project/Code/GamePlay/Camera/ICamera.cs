using _Project.Code.GamePlay.Character;
using UnityEngine;

namespace _Project.Code.GamePlay.Camera
{
    public interface ICamera
    {
        Quaternion Rotation { get; }

        void Init(ICharacter target);
    }
}