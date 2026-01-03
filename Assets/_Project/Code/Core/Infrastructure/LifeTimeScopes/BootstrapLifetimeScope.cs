using _Project.Code.Configs.Audio;
using _Project.Code.Core.AssetManagement;
using _Project.Code.Core.Factory;
using _Project.Code.Core.Infrastructure.StateMachine;
using _Project.Code.Core.Infrastructure.StateMachine.States;
using _Project.Code.Core.Services.Audio;
using _Project.Code.Core.Services.EventService;
using _Project.Code.Core.Services.Input;
using _Project.Code.Core.Services.SceneLoadService;
using _Project.Code.Core.Services.StaticData;
using _Project.Code.Util;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.Core.Infrastructure.LifeTimeScopes
{
    public class BootstrapLifetimeScope : LifetimeScope
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private CoroutineRunner _coroutineRunner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterAssetProvider(builder);
            RegisterGameStateMachine(builder);
            RegisterStates(builder);
            RegisterSceneLoader(builder);
            RegisterEventBus(builder);
            RegisterCoroutineRunner(builder);
            RegisterGameFactory(builder);
            RegisterLoadingCurtain(builder);
            RegisterAudioService(builder);
            RegisterInputService(builder);
            RegisterStaticDataService(builder);
            
            
            builder.RegisterEntryPoint<GameBootstrapper>();
        }

        private static void RegisterStaticDataService(IContainerBuilder builder) => 
            builder.Register<StaticDataService>(Lifetime.Singleton).AsImplementedInterfaces();

        private static void RegisterInputService(IContainerBuilder builder) =>
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();

        private void RegisterLoadingCurtain(IContainerBuilder builder) => 
            builder.RegisterComponentInNewPrefab(_loadingCurtain, Lifetime.Singleton).DontDestroyOnLoad();

        private static void RegisterAudioService(IContainerBuilder builder) =>
            builder.Register<AudioService>(Lifetime.Singleton).AsImplementedInterfaces();

        private static void RegisterGameFactory(IContainerBuilder builder) => 
            builder.Register<GameObjectBuilderFactory>(Lifetime.Singleton).AsImplementedInterfaces();

        private void RegisterAssetProvider(IContainerBuilder builder) => 
            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();

        private void RegisterCoroutineRunner(IContainerBuilder builder) => 
            builder.RegisterInstance(_coroutineRunner).As<ICoroutineRunner>();

        private static void RegisterEventBus(IContainerBuilder builder) => 
            builder.Register<EventBus>(Lifetime.Singleton).AsImplementedInterfaces();

        private static void RegisterSceneLoader(IContainerBuilder builder) =>
            builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();

        private void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<BootstrapState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LoadingLevelState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LoadingMenuState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void RegisterGameStateMachine(IContainerBuilder builder)
        {
            builder.Register<StatesFactory>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}