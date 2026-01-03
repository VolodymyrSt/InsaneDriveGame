using _Project.Code.Util;
using UnityEngine;
using VContainer;

namespace _Project.Code.Core.Services.EventService
{
    [RequireComponent(typeof(Collider))]
    public class CustomTrigger : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private Collider _collider;
        
        [Header("Setting")]
        [SerializeReference, SubclassSelector] private IEvent _event;
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeReference, SubclassSelector] private CustomTriggerCallback _triggerCallback;
        
        private IEventBus _eventBus;
        private ICoroutineRunner _coroutineRunner;
        
        [Inject]
        private void Construct(IEventBus eventBus, ICoroutineRunner coroutineRunner)
        {
            _eventBus = eventBus;
            _coroutineRunner = coroutineRunner;
        }

        private void OnValidate() => 
            _collider??= GetComponent<Collider>();

        private void Start()
        {
            _collider.isTrigger = true;
            _triggerCallback.Init(this, _coroutineRunner);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            _triggerCallback.OnTriggered();
            _eventBus.Publish(_event);
        }
    }
}