using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace _Project.Code.Core.Services.Input
{
    public class InputService : IInputService
    {
        public event Action OnClicked;
        public event Action OnClickCanceled;
        private readonly Input_Action _inputAction = new();
        
        public void Enable(bool value)
        {
            if (value)
            {
                _inputAction.Player.Click.started += OnMouseClicked;
                _inputAction.Player.Click.canceled += OnMouseClickCanceled;
                _inputAction.Enable();
            }
            else
            {
                _inputAction.Player.Click.started -= OnMouseClicked;
                _inputAction.Player.Click.canceled -= OnMouseClickCanceled;
                _inputAction.Disable();
            }
        }

        private void OnMouseClicked(InputAction.CallbackContext obj) => 
            OnClicked?.Invoke();

        private void OnMouseClickCanceled(InputAction.CallbackContext obj) => 
            OnClickCanceled?.Invoke();

        public bool PlayerJumpHeld() =>
            _inputAction.Player.Jump.IsPressed();
        
        public Vector2 GetPlayerMoveVector() => 
            _inputAction.Player.Move.ReadValue<Vector2>();
        
        public Vector2 GetPlayerLookVector() => 
            _inputAction.Player.Look.ReadValue<Vector2>();
    }
}