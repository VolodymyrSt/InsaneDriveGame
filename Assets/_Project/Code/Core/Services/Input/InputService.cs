using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace _Project.Code.Core.Services.Input
{
    public class InputService : IInputService
    {
        public event Action OnMousePressed;
        public event Action OnMousePressCanceled;
        
        private readonly Input_Action _inputAction;
        
        public InputService() => _inputAction = new Input_Action();
        
        public void Enable(bool value)
        {
            if (value)
            {
                _inputAction.Player.MousePress.performed += MousePressed;
                _inputAction.Player.MousePress.canceled += MousePressedCanceled;
                _inputAction.Enable();
            }
            else
            {
                _inputAction.Player.MousePress.performed -= MousePressed;
                _inputAction.Player.MousePress.canceled -= MousePressedCanceled;
                _inputAction.Disable();
            }
        }

        private void MousePressed(InputAction.CallbackContext obj)
        {
            Debug.Log("Mouse pressed");
            OnMousePressed?.Invoke();
        }

        private void MousePressedCanceled(InputAction.CallbackContext obj)
        {
            Debug.Log("MousePressedCanceled");
            OnMousePressCanceled?.Invoke();
        }

        public bool PlayerJumpHeld() =>
            _inputAction.Player.Jump.IsPressed();
        
        public bool PlayerCrouchHeld() =>
            _inputAction.Player.Crouch.IsPressed();
        
        public bool PlayerRunHeld() =>
            _inputAction.Player.Run.IsPressed();
        
        public Vector2 GetPlayerMoveVector() => 
            _inputAction.Player.Move.ReadValue<Vector2>();
        
        public Vector2 GetPlayerLookVector() => 
            _inputAction.Player.Look.ReadValue<Vector2>();
    }
}