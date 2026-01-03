namespace _Project.Code.Core.Services.EventService
{
    public interface IEventBus
    {
        void Subscribe<T>(CustomEvent<T> customEvent) where T : IEvent;
        void Unsubscribe<T>(CustomEvent<T> customEvent) where T : IEvent;
        void Publish(IEvent @event);
        void Clear();
    }
}