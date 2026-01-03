using _Project.Code.Core.Services.SceneLoadService;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Infrastructure.StateMachine.States
{
    public class LoadingLevelState : IEnterableState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        
        public LoadingLevelState(IGameStateMachine stateMachine, ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }
        
        public async UniTask Enter()
        {
            Debug.Log("LoadingLevelState");
            _sceneLoader.Load(SceneList.Level);
        }
    }
}