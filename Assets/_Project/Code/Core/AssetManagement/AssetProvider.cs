using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Code.Core.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCachedHandles = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _operationHandles = new();

        public async UniTask Initialize() =>
            await Addressables.InitializeAsync().ToUniTask();

        public async UniTask<TAsset> Load<TAsset>(AssetReference reference) where TAsset : class => 
            await Load<TAsset>(reference.AssetGUID);

        public async UniTask<TAsset> Load<TAsset>(string address) where TAsset : class
        {
            if (_completedCachedHandles.TryGetValue(address, out var completed))
                return (TAsset)completed.Result;

            var handle = Addressables.LoadAssetAsync<TAsset>(address);
            var result = await handle.ToUniTask();

            _completedCachedHandles[address] = handle;
            AddOperationHandle(address, handle);

            return result;
        }

        public void CleanUp()
        {
            foreach (var handles in _operationHandles.Values)
                foreach (var handle in handles)
                    Addressables.Release(handle);
            
            _completedCachedHandles.Clear();
            _operationHandles.Clear();
        }

        private void AddOperationHandle<T>(string address, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_operationHandles.TryGetValue(address, out var operationHandles))
            {
                operationHandles = new List<AsyncOperationHandle>();
                _operationHandles[address] = operationHandles;
            }

            operationHandles.Add(handle);
        }
    }
}