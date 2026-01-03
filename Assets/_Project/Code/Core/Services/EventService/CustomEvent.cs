using System;

namespace _Project.Code.Core.Services.EventService
{
    public interface ICustomEvent
    {
        void Invoke(IEvent @event);
    }
    
    public sealed class CustomEvent<T> : ICustomEvent where T : IEvent
    {
        private readonly Action<T> _onEvent;
        public CustomEvent(Action<T> onEvent) => 
            _onEvent = onEvent;
        
        public void Invoke(IEvent @event)
        {
            if (@event is T typed)
                _onEvent?.Invoke(typed);
        }
    }
}