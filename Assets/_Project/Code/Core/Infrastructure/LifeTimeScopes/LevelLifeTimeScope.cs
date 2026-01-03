using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using _Project.Code.GamePlay.Camera.Factory;
using _Project.Code.GamePlay.Character.Factory;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure.LifeTimeScopes
{
    public class LevelLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<EnterLevelState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<StatesFactory>(Lifetime.Singleton);
            
            builder.Register<PlayerFactory>(Lifetime.Singleton);
            builder.Register<CameraFactory>(Lifetime.Singleton);

            builder.RegisterEntryPoint<LevelBootstrapper>();
        }
    }
}