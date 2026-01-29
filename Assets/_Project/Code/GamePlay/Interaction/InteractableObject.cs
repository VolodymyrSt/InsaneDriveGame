using System;
using System.Collections;
using UnityEngine;
using VContainer;

namespace _Project.Code.GamePlay.Interaction
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        public event Action OnInteracted;
        
        [SerializeReference, SubclassSelector] private InteractionBehaviour _behaviour;
        [SerializeField] private Collider _collider;

        [Header("Description")]
        [SerializeField] private InteractableInfo _info;
        
        private IObjectResolver _resolver;
        private Coroutine _waitToEnableColliderCoroutine;
        
        public bool IsActivated => _collider.enabled;
        public InteractableInfo Info => _info;
        
        private void OnValidate() => 
            _collider ??= GetComponent<Collider>();

        [Inject]
        private void Construct(IObjectResolver resolver) => 
            _resolver = resolver;

        private void Start()
        {
            _behaviour.InitInfo(_info);
            _behaviour.Initialize(this);
            _behaviour.Initialize(_resolver);
        }

        public void Interact() => 
            _behaviour.Execute(OnInteracted);

        public void ToggleCollider(bool value) => 
            _collider.enabled = value;
        
        public void DisableAndEnableColliderAfterTime(float time, Action onComplete = null)
        {
            if (_waitToEnableColliderCoroutine != null)
                StopCoroutine(_waitToEnableColliderCoroutine);

            ToggleCollider(false);
            _waitToEnableColliderCoroutine = StartCoroutine(EnableColliderAfterTimeCoroutine(time, onComplete));
        }

        private IEnumerator EnableColliderAfterTimeCoroutine(float time, Action onComplete = null)
        {
            yield return new WaitForSeconds(time);
            ToggleCollider(true);
            onComplete?.Invoke();
        }

        private void OnDestroy()
        {
            if (_waitToEnableColliderCoroutine != null)
                StopCoroutine(_waitToEnableColliderCoroutine);
        }
    }
}