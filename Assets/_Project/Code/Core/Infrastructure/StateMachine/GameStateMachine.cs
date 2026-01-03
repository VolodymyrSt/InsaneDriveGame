using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine, ITickable
    {
        private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
        private readonly StatesFactory _stateFactory;
        private IState _currentState;

        public GameStateMachine(StatesFactory factory) => 
            _stateFactory = factory;

        public void Tick()
        {
            if (_currentState is ITickableState updatableState)
                updatableState.Tick();
        }

        public void Register<TState>(TState state) where TState : IState
        {
            var key = typeof(TState);
            if (!_states.ContainsKey(key))
                _states.Add(key, state);
            else
                Debug.LogWarning("You are trying to register a state of " + typeof(TState) + " as it is already registered.");
        }
        
        public void Unregister<TState>() where TState : IState
        {
            var key = typeof(TState);
            if (!_states.ContainsKey(key))
                Debug.LogWarning("You are trying to unregister not found state of " + typeof(TState));
            else
                _states.Remove(key);
        }

        public async UniTask Enter<TState>() where TState : class, IEnterableState
        {
            IEnterableState state = await ChangeState<TState>(); 
            await state.Enter();
        }

        public async UniTask Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayloadState<TPayLoad>
        {
            IPayloadState<TPayLoad> state = await ChangeState<TState>();
            await state.Enter(payLoad);
        }

        public async UniTask Enter<TState, TPayLoad, TTPayLoad>(TPayLoad payLoad1, TTPayLoad payLoad2) where TState : class, IPayloadState<TPayLoad, TTPayLoad>
        {
            IPayloadState<TPayLoad, TTPayLoad> state = await ChangeState<TState>();
            await state.Enter(payLoad1, payLoad2);
        }

        private async UniTask<TState> ChangeState<TState>() where TState : class, IState
        {
            if (_currentState is IExitableState exitable)
                await exitable.Exit();

            var newState = GetState<TState>();
            _currentState = newState;
            return newState;
        }
        private TState GetState<TState>() where TState : class, IState
        {
            if (_states.TryGetValue(typeof(TState), out var state))
                return state as TState;
            
            throw new Exception("State not found: " + typeof(TState));
        }
    }
}
