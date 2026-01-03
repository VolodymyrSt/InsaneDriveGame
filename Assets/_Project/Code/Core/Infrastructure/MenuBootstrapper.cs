using System;
using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure
{
    public class MenuBootstrapper : IInitializable, IDisposable, IStartable
    {
        private IGameStateMachine _gameStateMachine;
        private StatesFactory _stateFactory;
        
        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _gameStateMachine = gameStateMachine;
            _stateFactory = statesFactory;
        }

        public void Initialize()
        {
            _gameStateMachine.Register(_stateFactory.GetState<EnterMenuState>());
        }
        
        public void Dispose()
        {
            _gameStateMachine.Unregister<EnterMenuState>();
        }

        public void Start()
        {
            _gameStateMachine.Enter<EnterMenuState>();
        }
    }
}