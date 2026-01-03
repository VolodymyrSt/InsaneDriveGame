using _Project.Code.Core.AssetManagement;
using _Project.Code.GamePlay.Character;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.GamePlay.Camera.Factory
{
    public class CameraFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;
        
        public CameraFactory(IAssetProvider assetProvider, IObjectResolver objectResolver)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
        }

        public async UniTask<ICamera> CreateCamera()
        {
            var prefab = await _assetProvider.Load<GameObject>(AssetsAddress.Camera);
            var instance = _objectResolver.Instantiate(prefab);
            return instance.GetComponent<CameraHandler>();
        }
    }
}