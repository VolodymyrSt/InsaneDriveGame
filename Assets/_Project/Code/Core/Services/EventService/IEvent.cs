using System;
using _Project.Code.GamePlay.Interaction;

namespace _Project.Code.Core.Services.EventService
{
    public interface IEvent { }

    [Serializable]
    public class OnInteractableFound : IEvent
    {
        public IInteractable Interactable;
        
        public OnInteractableFound(IInteractable interactable) => 
            Interactable = interactable;
    }
    
    [Serializable]
    public class OnCustomTriggerTriggered : IEvent {}
    
    [Serializable]
    public class OnInteractableLost : IEvent {}
}