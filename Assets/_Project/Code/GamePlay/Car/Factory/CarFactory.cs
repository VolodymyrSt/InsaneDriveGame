using _Project.Code.Core.AssetManagement;
using _Project.Code.Core.Services.ContentProviderService;
using _Project.Code.GamePlay.CameraLogic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.GamePlay.Car.Factory
{
    public class CarFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;
        private readonly IContentProvider _contentProvider;
        
        public CarFactory(IAssetProvider assetProvider, IObjectResolver objectResolver
        , IContentProvider contentProvider)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
            _contentProvider = contentProvider;
        }

        public async UniTask<ICar> CreateCar(Vector3 at)
        {
            var prefab = await _assetProvider.Load<GameObject>(AssetsAddress.Car);
            var instance = _objectResolver.Instantiate(prefab, at, Quaternion.identity);
            var car = instance.GetComponent<CarMediator>();
            _contentProvider.SetCar(car);
            return car;
        }
    }
}