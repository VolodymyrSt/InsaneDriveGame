using System;
using UnityEngine;

namespace _Project.Code.Core.Services.Input
{
    public interface IInputService
    {
        event Action OnPlayerInteracted;
        
        void Enable(bool value);
        Vector2 GetPlayerMoveVector();
        Vector2 GetPlayerLookVector();
        bool PlayerJumpHeld();
        bool PlayerCrouchHeld();
        bool PlayerSprintHeld();
        Vector2 GetCarDriveVector();
    }
}