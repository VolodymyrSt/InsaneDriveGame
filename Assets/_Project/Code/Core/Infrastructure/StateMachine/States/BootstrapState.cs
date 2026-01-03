using _Project.Code.Core.AssetManagement;
using _Project.Code.Core.Services.SceneLoadService;
using _Project.Code.Core.Services.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Infrastructure.StateMachine.States
{
    public class BootstrapState : IEnterableState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public BootstrapState(ISceneLoader sceneLoader, IGameStateMachine stateMachine
        , IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public async UniTask Enter()
        {
            await _assetProvider.Initialize();
            await _staticDataService.Initialize();
            
            _stateMachine.Enter<LoadingMenuState>().Forget();
        }
    }
}