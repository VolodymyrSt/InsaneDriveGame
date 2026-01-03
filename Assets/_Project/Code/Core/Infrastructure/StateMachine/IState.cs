using Cysharp.Threading.Tasks;

namespace _Project.Code.Core.Infrastructure.StateMachine
{
    public interface IState { }
    
    public interface IEnterableState : IState
    {
        UniTask Enter();
    }
    
    public interface IAsyncEnterableState : IState
    {
        UniTask Enter();
    }

    public interface IPayloadState<TPayload> : IState
    {
        UniTask Enter(TPayload payload);
    }

    public interface IPayloadState<TPayload, TTPayload> : IState
    {
        UniTask Enter(TPayload payload1, TTPayload payload2);
    } 
    
    public interface ITickableState : IState
    {
        void Tick();
    }

    public interface IExitableState : IState
    {
        UniTask Exit();
    }
}