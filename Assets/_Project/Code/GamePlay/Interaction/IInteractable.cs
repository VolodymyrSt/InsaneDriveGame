using System;

namespace _Project.Code.GamePlay.Interaction
{
    public interface IInteractable
    {
        event Action OnInteracted;

        bool IsActivated { get; }
        InteractableInfo Info { get; }
        void Interact();
        void ToggleCollider(bool value);
        void DisableAndEnableColliderAfterTime(float time, Action onComplete = null);
    }
}