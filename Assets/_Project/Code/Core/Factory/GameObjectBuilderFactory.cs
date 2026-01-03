using System.Threading.Tasks;
using _Project.Code.Core.AssetManagement;
using _Project.Code.Core.Services.Audio;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace _Project.Code.Core.Factory
{
    public class GameObjectBuilderFactory : IGameObjectBuilderFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;

        public GameObjectBuilderFactory(IAssetProvider assetProvider, IObjectResolver objectResolver)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
        }
        
        public EmptyGameObjectBuilder<T> BuildNewFor<T>(string name) where T : Component => 
            new(name, _objectResolver);
    }
}