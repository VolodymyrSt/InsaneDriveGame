using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure.LifeTimeScopes
{
    public class MenuLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<EnterMenuState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<StatesFactory>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<MenuBootstrapper>();
        }
    }
}