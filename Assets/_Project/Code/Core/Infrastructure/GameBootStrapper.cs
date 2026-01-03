using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using _Project.Code.Core.Services.SceneLoadService;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure
{
    public class GameBootstrapper : IInitializable
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly StatesFactory _stateFactory;
        private readonly ISceneLoader _sceneLoader;
        
        public GameBootstrapper(IGameStateMachine gameStateMachine, StatesFactory statesFactory
        , ISceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _stateFactory = statesFactory;
            _sceneLoader = sceneLoader;
        }

        public void Initialize() =>
            RunGame();

        private void RunGame() => 
            _sceneLoader.Load(SceneList.Bootstrap, OnBootstrapSceneLoaded);

        private void OnBootstrapSceneLoaded()
        {
            _gameStateMachine.Register(_stateFactory.GetState<BootstrapState>());
            _gameStateMachine.Register(_stateFactory.GetState<LoadingLevelState>());
            _gameStateMachine.Register(_stateFactory.GetState<LoadingMenuState>());
            
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}