using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Code.Core.AssetManagement
{
    public interface IAssetProvider
    {
        UniTask Initialize();
        UniTask<T> Load<T>(AssetReference reference) where T : class;
        UniTask<TAsset> Load<TAsset>(string address) where TAsset : class;
        void CleanUp();
    }
}