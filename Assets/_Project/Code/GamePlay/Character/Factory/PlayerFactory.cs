using _Project.Code.Core.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.GamePlay.Character.Factory
{
    public class PlayerFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;
        
        public PlayerFactory(IAssetProvider assetProvider, IObjectResolver objectResolver)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
        }

        public async UniTask<ICharacter> CreateCharacter(Vector3 at)
        {
            var prefab = await _assetProvider.Load<GameObject>(AssetsAddress.Character);
            var instance = _objectResolver.Instantiate(prefab, at, Quaternion.identity);
            return instance.GetComponent<Character>();
        }
    }
}