using System;

namespace _Project.Code.GamePlay.Interaction
{
    public interface IInteractable
    {
        event Action OnInteracted;
        InteractableType Type { get; }
        bool IsActivated { get; }
        void Interact();
    }
}