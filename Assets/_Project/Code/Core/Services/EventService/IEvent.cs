using System;

namespace _Project.Code.Core.Services.EventService
{
    public interface IEvent { }

    [Serializable]
    public class OnPlayerDied : IEvent
    {
        public int PlayerId;
    }
    
    [Serializable]
    public class OnCustomTriggerTriggered : IEvent {}
}