using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace _Project.Code.Core.Services.Input
{
    public class InputService : IInputService
    {
        public event Action OnMousePressed;
        public event Action OnPlayerInteracted;
        public event Action OnMousePressCanceled;
        
        private readonly Input_Action _inputAction;
        
        public InputService() => _inputAction = new Input_Action();
        
        public void Enable(bool value)
        {
            if (value)
            {
                _inputAction.Player.MousePress.performed += MousePressed;
                _inputAction.Player.MousePress.canceled += MousePressedCanceled;
                _inputAction.Player.Interact.started += PlayerInteracted;
                _inputAction.Enable();
            }
            else
            {
                _inputAction.Player.MousePress.performed -= MousePressed;
                _inputAction.Player.MousePress.canceled -= MousePressedCanceled;
                _inputAction.Player.Interact.started -= PlayerInteracted;
                _inputAction.Disable();
            }
        }

        private void MousePressed(InputAction.CallbackContext obj) => OnMousePressed?.Invoke();
        private void MousePressedCanceled(InputAction.CallbackContext obj) => OnMousePressCanceled?.Invoke();
        
        private void PlayerInteracted(InputAction.CallbackContext obj)
        {
            Debug.Log("Player Interacted");
            OnPlayerInteracted?.Invoke();
        }

        public bool PlayerJumpHeld() =>
            _inputAction.Player.Jump.IsPressed();
        
        public bool PlayerCrouchHeld() =>
            _inputAction.Player.Crouch.IsPressed();
        
        public bool PlayerSprintHeld() =>
            _inputAction.Player.Sprint.IsPressed();
        
        public Vector2 GetCarDriveVector() => 
            _inputAction.Car.Drive.ReadValue<Vector2>();
        
        
        public Vector2 GetPlayerMoveVector() => 
            _inputAction.Player.Move.ReadValue<Vector2>();
        
        public Vector2 GetPlayerLookVector() => 
            _inputAction.Player.Look.ReadValue<Vector2>();
    }
}