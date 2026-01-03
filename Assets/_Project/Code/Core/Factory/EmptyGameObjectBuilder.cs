using System;
using System.Collections.Generic;
using _Project.Code.Core.Factory.Pool;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace _Project.Code.Core.Factory
{
    public class EmptyGameObjectBuilder<T> where T : Component
    {
        private readonly string _name;
        private readonly IObjectResolver _objectResolver;
        
        private readonly List<Type> _components = new List<Type>();
        private Vector3 _position = Vector3.zero;
        private Transform _under = null;
        private Quaternion _rotation = Quaternion.identity;
        private bool _dontDestroyOnLoad = false;

        public EmptyGameObjectBuilder(string name, IObjectResolver objectResolver)
        {
            _name = name;
            _objectResolver = objectResolver;
            TryAddComponent<T>();
        }
  
        public EmptyGameObjectBuilder<T> At(Vector3 at)
        {
            _position = at;
            return this;
        }

        public EmptyGameObjectBuilder<T> Under(Transform under)
        {
            _under = under;
            return this;
        }

        public EmptyGameObjectBuilder<T> Rotated(Quaternion rotation)
        {
            _rotation = rotation;
            return this;
        }
        
        public EmptyGameObjectBuilder<T> AsPersist()
        {
            _dontDestroyOnLoad = true;
            return this;
        }

        public EmptyGameObjectBuilder<T> With<TComponent>() where TComponent : Component
        {
            TryAddComponent<TComponent>();
            return this;
        }
        
        public T Create(Action<T> onCreated = null)
        {
            GameObject gameObject = new(_name, _components.ToArray()) {
                transform =
                {
                    position = _position,
                    rotation = _rotation,
                    parent = _under
                }
            };
            
            if (_dontDestroyOnLoad) 
                Object.DontDestroyOnLoad(gameObject);
            
            _objectResolver.Inject(gameObject);

            var componentFor = gameObject.GetComponent<T>();
            onCreated?.Invoke(componentFor);
            return componentFor;
        }
        
        private void TryAddComponent<TComponent>() where TComponent : Component
        {
            if (!_components.Contains(typeof(TComponent)))
                _components.Add(typeof(TComponent));
            else
                Debug.LogWarning("You are trying to add second component of type " + typeof(TComponent).FullName + " to " + _name);
        }
    }
}