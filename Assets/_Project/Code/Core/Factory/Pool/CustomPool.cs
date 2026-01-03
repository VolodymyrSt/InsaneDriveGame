using System;
using System.Collections.Generic;

namespace _Project.Code.Core.Factory.Pool
{
    public class CustomPool<TPoolable, TData> 
        where TData : PoolableData 
        where TPoolable : IPoolable<TData>
    {
        private readonly Queue<TPoolable> _pooledObjects;
        private readonly List<TPoolable> _activePooledObjects;
        private readonly Func<TPoolable> _createPoolable;
        private readonly int _initialCapacity;
        public bool IsEmpty => _pooledObjects.Count == 0;
        public int ActiveCount => _activePooledObjects.Count;

        public CustomPool(Func<TPoolable> createPoolable, int initialCapacity = 0)
        {
            _pooledObjects = new Queue<TPoolable>();
            _activePooledObjects = new List<TPoolable>();
            
            _createPoolable = createPoolable;
            _initialCapacity = initialCapacity;
        }

        public void Preload()
        {
            for (var i = 0; i < _initialCapacity; i++)
            {
                var poolable = _createPoolable.Invoke();
                _pooledObjects.Enqueue(poolable);
            }
        }
        
        public TPoolable Get(TData data)
        {
            var poolable = IsEmpty ? _createPoolable.Invoke() : _pooledObjects.Dequeue();
            poolable.OnSpawn(data);
            _activePooledObjects.Add(poolable);
            return poolable;
        }

        public void Release(TPoolable poolable)
        {
            poolable.OnDespawn();
            _activePooledObjects.Remove(poolable);
            _pooledObjects.Enqueue(poolable);
        }

        public void Clear()
        {
            while (_pooledObjects.Count > 0)
            {
                var poolable = _pooledObjects.Dequeue();
                poolable.OnDestroySelf();
            }
            
            _pooledObjects.Clear();
            _activePooledObjects.Clear();
        }
        
        public List<TPoolable> GetActivePoolable() =>
            _activePooledObjects;
    }
}