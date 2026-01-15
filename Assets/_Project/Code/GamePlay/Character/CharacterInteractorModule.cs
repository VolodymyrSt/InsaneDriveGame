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
        
        public CharacterInteractorModule(ICamera camera, Transform cameraTarget, CharacterConfigSO config
            , LayerMask interactionLayer, IEventBus eventBus)
        {
            _camera = camera;
            _cameraTarget = cameraTarget;
            _interactionLayer = interactionLayer;
            _eventBus = eventBus;

            _interactionDistance = config.InteractionDistance;
        }

        public void UpdateTarget()
        {
            if (Physics.Raycast(_cameraTarget.position, _camera.Transform.forward, out var hit, _interactionDistance, _interactionLayer))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                    SetTarget(interactable);
                else
                    TryCleanTarget();
            }
            else
                TryCleanTarget();
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
    }
}