using _Project.Code.Configs.Character;
using _Project.Code.Core.Services.EventService;
using _Project.Code.GamePlay.CameraLogic;
using _Project.Code.GamePlay.Interaction;
using UnityEngine;

namespace _Project.Code.GamePlay.Character
{
    public class CharacterInteractorModule
    {
        private readonly ICamera _camera;
        private readonly IEventBus _eventBus;
        private readonly Transform _cameraTarget;
        private readonly LayerMask _interactionLayer;
        
        private readonly float _interactionDistance;
        
        private IInteractable _targetInteractable;
        private IGrabInteractable _currentGrabInteractable;
        private bool _requestedMousePressed;
        
        public CharacterInteractorModule(ICamera camera, Transform cameraTarget, CharacterConfigSO config
            , LayerMask interactionLayer, IEventBus eventBus)
        {
            _camera = camera;
            _cameraTarget = cameraTarget;
            _interactionLayer = interactionLayer;
            _eventBus = eventBus;

            _interactionDistance = config.InteractionDistance;
        }

        public void RequestInput(bool requestedMousePressed) => 
            _requestedMousePressed = requestedMousePressed;

        public void UpdateTarget()
        {
            if (RequestedInteraction(out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                    SetTarget(interactable);
                else
                    TryCleanTarget();
            }
            else
                TryCleanTarget();
        }


        public void TryGrabSmth()
        {
            if (_requestedMousePressed)
            {
                if (RequestedInteraction(out RaycastHit hit))
                {
                    if (hit.collider.TryGetComponent(out IGrabInteractable interactable))
                    {
                        if (_currentGrabInteractable == interactable || 
                            _currentGrabInteractable != null) return;
                        
                        _currentGrabInteractable = interactable;
                        interactable.Grab(); 

                        Debug.Log("Interacting with " + _currentGrabInteractable);
                    }
                }
            }
            else
            {
                if (_currentGrabInteractable == null) return;
                
                _currentGrabInteractable.Release();
                _currentGrabInteractable = null;
            }
        }

        public void TryInteract() => 
            _targetInteractable?.Interact();
        
        private void TryCleanTarget()
        {
            if (_targetInteractable != null)
            {
                _targetInteractable = null;
                _eventBus.Publish(new OnInteractableLost());
            }
        }

        private void SetTarget(IInteractable interactable)
        {
            if (_targetInteractable == interactable)
                return;

            _targetInteractable = interactable;
            _eventBus.Publish(new OnInteractableFound(interactable));
        }
        
        private bool RequestedInteraction(out RaycastHit hit) =>
            Physics.Raycast(_cameraTarget.position, _camera.Transform.forward, out hit, _interactionDistance, _interactionLayer);
    }
}
