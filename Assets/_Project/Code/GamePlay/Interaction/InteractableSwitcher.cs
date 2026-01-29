using UnityEngine;

namespace _Project.Code.GamePlay.Interaction
{
    public class InteractableSwitcher : MonoBehaviour
    {
        [SerializeField] private InteractableObject _firstInteractable;
        [SerializeField] private InteractableObject _secondInteractable;

        public bool _withFirstActive = true;

        private void Start()
        {
            SetActiveInteractable(_withFirstActive ? _firstInteractable : _secondInteractable,
                _withFirstActive ? _secondInteractable : _firstInteractable);
            
            _firstInteractable.OnInteracted += SwapActiveInteractable;
            _secondInteractable.OnInteracted += SwapActiveInteractable;
        }

        private void SwapActiveInteractable()
        {
            var isFirstActive = _firstInteractable.IsActivated;
            
            SetActiveInteractable(isFirstActive ? _secondInteractable : _firstInteractable, 
                isFirstActive ? _firstInteractable : _secondInteractable );
        }
        
        private void SetActiveInteractable(InteractableObject toEnable, InteractableObject toDisable)
        {
            toEnable.ToggleCollider(true);
            toDisable.ToggleCollider(false);
        }

        private void OnDestroy()
        {
            _firstInteractable.OnInteracted -= SwapActiveInteractable;
            _secondInteractable.OnInteracted -= SwapActiveInteractable;
        }
    }
}