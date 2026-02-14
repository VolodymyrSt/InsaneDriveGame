using System;
using UnityEngine;

namespace _Project.Code.Core.Services.Input
{
    public interface IInputService
    {
        event Action OnPlayerInteracted;
        bool IsMousePressed { get; }

        void Enable(bool value);
        Vector2 GetMouseDelta();
        Vector2 GetCharacterMoveVector();
        Vector2 GetCharacterLookVector();
        bool PlayerJumpHeld();
        bool PlayerCrouchHeld();
        bool PlayerSprintHeld();
        float GetCarSteeringWheelTurnAxis();
        float GetCarGasInput();
        float GetCarClutchInput();
        float GetCarBreakInput();
    }
}