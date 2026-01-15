using System;
using UnityEngine;
using VContainer;

namespace _Project.Code.GamePlay.Interaction
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        public event Action OnInteracted;
        
        [SerializeField] protected InteractableType _interactableType;
        [SerializeReference, SubclassSelector] private InteractionBehaviour _behaviour;
        [SerializeField] protected Collider _collider;
        
        private IObjectResolver _resolver;

        public InteractableType Type => _interactableType;
        public bool IsActivated => _collider.enabled;
        
        private void OnValidate() => 
            _collider ??= GetComponent<Collider>();

        [Inject]
        private void Construct(IObjectResolver resolver) => 
            _resolver = resolver;

        private void Start() => 
            _behaviour.Initialize(_resolver);

        public void Interact() => 
            _behaviour.Execute(OnInteracted);

        public void Toggle(bool value) => 
            _collider.enabled = value;
    }
}