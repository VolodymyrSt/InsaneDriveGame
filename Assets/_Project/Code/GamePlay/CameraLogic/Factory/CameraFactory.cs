using _Project.Code.Core.AssetManagement;
using _Project.Code.Core.Services.ContentProviderService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.GamePlay.CameraLogic.Factory
{
    public class CameraFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;
        private readonly IContentProvider _contentProvider;
        
        public CameraFactory(IAssetProvider assetProvider, IObjectResolver objectResolver
        , IContentProvider contentProvider)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
            _contentProvider = contentProvider;
        }

        public async UniTask<ICamera> CreateCamera()
        {
            var prefab = await _assetProvider.Load<GameObject>(AssetsAddress.Camera);
            var instance = _objectResolver.Instantiate(prefab);
            var camera = instance.GetComponent<CameraHandler>();
            _contentProvider.SetCamera(camera);
            return camera;
        }
    }
}