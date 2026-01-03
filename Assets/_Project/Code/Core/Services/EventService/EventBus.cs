using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Code.Core.Services.EventService
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<ICustomEvent>> _signals = new();

        public void Subscribe<T>(CustomEvent<T> customEvent) where T : IEvent
        {
            var key = typeof(T);

            if (!_signals.TryGetValue(key, out var _))
                _signals[key] = new List<ICustomEvent>();
            
            _signals[key].Add(customEvent);
        }

        public void Unsubscribe<T>(CustomEvent<T> customEvent) where T : IEvent
        {
            var key = typeof(T);

            if (!_signals.TryGetValue(key, out var bindings))
            {
                Debug.LogWarning($"Can't unsubscribe signal: {key}");
                return;
            }

            if (!bindings.Remove(customEvent))
            {
                Debug.LogWarning($"Signal binding not found: {key}");
                return;
            }

            if (bindings.Count == 0)
                _signals.Remove(key);
        }

        public void Publish(IEvent @event)
        {
            var key = @event.GetType();

            if (!_signals.TryGetValue(key, out var signals))
            {
                Debug.LogWarning("Can`t invoke signal: " + key);
                return;
            }
            
            foreach (var signal in signals)
                signal?.Invoke(@event);
        }

        public void Clear() => 
            _signals.Clear();
    }
}