using System;
using _Project.Code.Core.Services.EventService;
using UnityEngine;

namespace _Project.Tests
{
    public class TestDI : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private IEvent customEvent;
        
        private EventBus _eventBus;

        private CustomEvent<OnPlayerDied> _event;
        
        private void Awake()
        {
            _eventBus = new EventBus();

            _event = new CustomEvent<OnPlayerDied>(OnPlayerDied);
            _eventBus.Subscribe(_event);

            // _container.Bind<B>().AsSingle().NotLazy();
            // _container.BindService<IA, A>().AsSingle();
            // _container.Bind<C>().AsSingle();
        }

        private void Start()
        {
            _eventBus.Publish(new OnPlayerDied());
        }

        private void Update()
        {
            // if (Input.GetKey(KeyCode.C))
            //     _container.Resolve<C>().DoSmthAll();
            
            if (Input.GetKeyDown(KeyCode.C))
                _eventBus.Publish(customEvent);
        }

        private void OnPlayerDied(OnPlayerDied signal) => 
            Debug.Log("OnPlayerDied" + signal.PlayerId);

        private void OnDestroy()
        {
            _eventBus.Unsubscribe(_event);
        }
    }

    public class D : MonoBehaviour
    {
        public void Initialize()
        {
            Debug.Log("Initialize D");
        }
    }

    public class B
    {
        public B(){}

        public void DoSmth() => 
            Debug.Log("DoSmth B");

        public void Initialize()
        {
            Debug.Log("Initialize B");
        }
    }

    public interface IA
    {
        void DoSmth();
    }

    public class A
    {
        public A(){}

        public void DoSmth() => 
            Debug.Log("DoSmth A");
        
        public void Initialize()
        {
            Debug.Log("Initialize A");
        }
    }
    
    public class C
    {
        private B _b;
        private IA _a;

        public C(B b, IA a)
        {
            _b = b;
            _a = a;
        }
        
        public C(B b)
        {
            _b = b;
        }

        public void DoSmthAll()
        {
            _b.DoSmth();
            _a.DoSmth();
        }
    }
}