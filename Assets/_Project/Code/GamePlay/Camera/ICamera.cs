using _Project.Code.GamePlay.Character;
using UnityEngine;

namespace _Project.Code.GamePlay.Camera
{
    public interface ICamera
    {
        void Init(Transform target);

        Vector3 GetNormalizedForward();
        Vector3 GetNormalizedRight();
    }
}