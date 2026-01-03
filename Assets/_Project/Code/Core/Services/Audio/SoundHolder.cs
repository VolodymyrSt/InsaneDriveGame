using UnityEngine;

namespace _Project.Code.Core.Services.Audio
{
    public class SoundHolder : MonoBehaviour 
    {
        public void ReturnToPlace(SoundEmitter soundEmitter)
        {
            soundEmitter.transform.SetParent(transform, false);
            soundEmitter.transform.localPosition = Vector3.zero;
        }

        public Transform GetPlace() => transform;
    }
}
