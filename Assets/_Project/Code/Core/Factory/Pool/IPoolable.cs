namespace _Project.Code.Core.Factory.Pool
{
    public interface IPoolable<TPoolableData> where TPoolableData : PoolableData
    {
        void OnSpawn(TPoolableData data);
        void OnDespawn();
        void OnDestroySelf();
    }
    
    public interface IPoolable<TKey, TPoolableData> where TKey : class where TPoolableData : PoolableData
    {
        TKey Key { get; }
        
        void OnSpawn(TPoolableData data);
        void OnDespawn();
        void OnDestroySelf();
    }
}