
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure.StateMachine
{
    public class StatesFactory
    {
        private readonly IObjectResolver _resolver;
        
        public StatesFactory(IObjectResolver resolver) => 
            _resolver = resolver;

        public TState GetState<TState>() where TState : IState => 
            _resolver.Resolve<TState>();
    }
}