using System;
using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure
{
    public class LevelBootstrapper : IInitializable, IStartable, IDisposable
    {
        private IGameStateMachine _gameStateMachine;
        private StatesFactory _stateFactory;
        
        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _gameStateMachine = gameStateMachine;
            _stateFactory = statesFactory;
        }

        public void Initialize() =>
            _gameStateMachine.Register(_stateFactory.GetState<EnterLevelState>());

        public void Start() => 
            _gameStateMachine.Enter<EnterLevelState>();
        
        public void Dispose()
        {
            _gameStateMachine.Unregister<EnterLevelState>();
        }

    }
}