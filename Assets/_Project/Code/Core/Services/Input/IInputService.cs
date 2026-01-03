using System;
using UnityEngine;

namespace _Project.Code.Core.Services.Input
{
    public interface IInputService
    {
        void Enable(bool value);
        Vector2 GetPlayerMoveVector();
        Vector2 GetPlayerLookVector();
        bool PlayerJumpHeld();
    }
}