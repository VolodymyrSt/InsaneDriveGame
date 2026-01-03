using Cysharp.Threading.Tasks;

namespace _Project.Code.Core.Infrastructure.StateMachine
{
    public interface IGameStateMachine
    {
        void Register<TState>(TState state) where TState : IState;
        UniTask Enter<TState>() where TState : class, IEnterableState;
        UniTask Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayloadState<TPayLoad>;
        UniTask Enter<TState, TPayLoad, TTPayLoad>(TPayLoad payLoad1, TTPayLoad payLoad2) where TState : class, IPayloadState<TPayLoad, TTPayLoad>;
        void Unregister<TState>() where TState : IState;
    }
}