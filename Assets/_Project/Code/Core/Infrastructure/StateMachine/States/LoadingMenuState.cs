using _Project.Code.Core.Services.SceneLoadService;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Infrastructure.StateMachine.States
{
    public class LoadingMenuState : IEnterableState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        
        public LoadingMenuState(IGameStateMachine stateMachine, ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }
        
        public async UniTask Enter()
        {
            Debug.Log("LoadingMenuState");
            _sceneLoader.Load(SceneList.Menu);
        }
    }
}