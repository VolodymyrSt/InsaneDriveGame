using _Project.Code.Core.AssetManagement;
using _Project.Code.Core.Services.ContentProviderService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.GamePlay.Character.Factory
{
    public class CharacterFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;
        private readonly IContentProvider _contentProvider;
        
        public CharacterFactory(IAssetProvider assetProvider, IObjectResolver objectResolver
        , IContentProvider contentProvider)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
            _contentProvider = contentProvider;
        }

        public async UniTask<ICharacter> CreateCharacter(Vector3 at)
        {
            var prefab = await _assetProvider.Load<GameObject>(AssetsAddress.Character);
            var instance = _objectResolver.Instantiate(prefab, at, Quaternion.identity);
            var character = instance.GetComponent<Character>();
            _contentProvider.SetCharacter(character);
            return character;
        }
    }
}