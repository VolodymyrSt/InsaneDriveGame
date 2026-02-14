using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace _Project.Code.Core.Services.Input
{
    public class InputService : IInputService
    {
        public event Action OnPlayerInteracted;
        
        private readonly Input_Action _inputAction;
        private bool _isMousePressed = false;
        
        public bool IsMousePressed => _isMousePressed;
        
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

        private void MousePressed(InputAction.CallbackContext obj) => 
            _isMousePressed = true;

        private void MousePressedCanceled(InputAction.CallbackContext obj) => 
            _isMousePressed = false;

        private void PlayerInteracted(InputAction.CallbackContext obj) => 
            OnPlayerInteracted?.Invoke();

        public bool PlayerJumpHeld() =>
            _inputAction.Player.Jump.IsPressed();
        
        public bool PlayerCrouchHeld() =>
            _inputAction.Player.Crouch.IsPressed();
        
        public bool PlayerSprintHeld() =>
            _inputAction.Player.Sprint.IsPressed();
        
        public float GetCarSteeringWheelTurnAxis() => 
            _inputAction.Car.SteeringWheel.ReadValue<float>();   
        
        public float GetCarGasInput() => 
            _inputAction.Car.Gas.ReadValue<float>();
        
        public float GetCarClutchInput() => 
            _inputAction.Car.Clutch.ReadValue<float>();  
        
        public float GetCarBreakInput() => 
            _inputAction.Car.Break.ReadValue<float>();
        
        public Vector2 GetCharacterMoveVector() => 
            _inputAction.Player.Move.ReadValue<Vector2>();
        
        public Vector2 GetCharacterLookVector() => 
            _inputAction.Player.Look.ReadValue<Vector2>();
        
        public Vector2 GetMouseDelta() => 
            _inputAction.Player.MouseDelta.ReadValue<Vector2>();
    }
}
