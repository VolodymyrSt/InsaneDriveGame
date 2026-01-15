using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using _Project.Code.Core.Services.ContentProviderService;
using _Project.Code.Core.Services.EventService;
using _Project.Code.GamePlay.CameraLogic.Factory;
using _Project.Code.GamePlay.Car.Factory;
using _Project.Code.GamePlay.Character.Factory;
using _Project.Code.GamePlay.UI;
using _Project.Code.GamePlay.UI.ToolTip;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure.LifeTimeScopes
{
    public class LevelLifeTimeScope : LifetimeScope
    {
        [SerializeField] private Camera _camera;
        
        [Header("ToolTip")]
        [SerializeField] private ToolTipView _toolTipView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEventBus(builder);
            RegisterCamera(builder);
            
            builder.Register<EnterLevelState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<StatesFactory>(Lifetime.Singleton);
            
            RegisterToolTipView(builder);
            RegisterLevelUIMediator(builder);
            
            RegisterContentProvider(builder);
            RegisterFactories(builder);

            builder.RegisterEntryPoint<LevelBootstrapper>();
        }

        private void RegisterToolTipView(IContainerBuilder builder) => 
            builder.RegisterInstance(_toolTipView);

        private void RegisterLevelUIMediator(IContainerBuilder builder) => 
            builder.Register<LevelUIMediator>(Lifetime.Singleton).AsImplementedInterfaces();

        private void RegisterContentProvider(IContainerBuilder builder) => 
            builder.Register<ContentProvider>(Lifetime.Singleton).AsImplementedInterfaces();
        
        private void RegisterEventBus(IContainerBuilder builder) => 
            builder.Register<EventBus>(Lifetime.Singleton).AsImplementedInterfaces();

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<CharacterFactory>(Lifetime.Singleton);
            builder.Register<CameraFactory>(Lifetime.Singleton);
            builder.Register<CarFactory>(Lifetime.Singleton);
        }

        private void RegisterCamera(IContainerBuilder builder) => 
            builder.RegisterInstance(_camera);
    }
}