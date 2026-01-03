using System;
using System.Collections.Generic;

namespace _Project.Code.Core.Factory.Pool
{
    public class CustomMultiPool<TKey, TPoolable, TData> 
        where TData : PoolableData 
        where TKey : class 
        where TPoolable : IPoolable<TKey, TData>
    {
        private readonly Dictionary<TKey, Queue<TPoolable>> _pooledObjects;
        private readonly Func<TKey, TPoolable> _createPoolable;
        private readonly int _initialPerObjectCapacity;

        public CustomMultiPool(Func<TKey, TPoolable> createPoolable, int initialPerObjectCapacity = 0)
        {
            _pooledObjects = new Dictionary<TKey, Queue<TPoolable>>();
            
            _createPoolable = createPoolable;
            _initialPerObjectCapacity = initialPerObjectCapacity;
        }

        public void Preload(TKey key)
        {
            for (var i = 0; i < _initialPerObjectCapacity; i++)
            {
                var poolable = _createPoolable.Invoke(key);

                if (!_pooledObjects.TryGetValue(key, out Queue<TPoolable> pooledObjects))
                {
                    pooledObjects = new Queue<TPoolable>();
                    _pooledObjects.Add(key, pooledObjects);
                }

                pooledObjects.Enqueue(poolable);
            }
        }
        
        public TPoolable Get(TKey key, TData data)
        {
            TPoolable poolable;
            if (_pooledObjects.TryGetValue(key, out Queue<TPoolable> pooledObjects) && pooledObjects.Count > 0)
                poolable = pooledObjects.Dequeue();
            else
                poolable = _createPoolable.Invoke(key);
            
            poolable.OnSpawn(data);
            return poolable;
        }

        public void Release(TPoolable poolable)
        {
            var key = poolable.Key;
            
            if (!_pooledObjects.TryGetValue(key, out Queue<TPoolable> pooledObjects))
            {
                pooledObjects = new Queue<TPoolable>();
                _pooledObjects.Add(key, pooledObjects);
            }
            
            poolable.OnDespawn();
            pooledObjects.Enqueue(poolable);
        }

        public void Clear()
        {
            foreach (var pooledObjects in _pooledObjects.Values)
                foreach (var poolable in pooledObjects)
                    poolable.OnDestroySelf();

            _pooledObjects.Clear();
        }
    }
}