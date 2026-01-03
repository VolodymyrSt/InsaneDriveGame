using System;
using _Project.Code.Configs.Character;
using _Project.Code.Core.Services.Input;
using _Project.Code.Util;
using UnityEngine;

namespace _Project.Code.GamePlay.Character
{
    public class JumpPerformer 
    {
        private readonly CharacterController _characterController;
        private readonly GravityApplier _gravityApplier;
        private readonly IInputService _inputService;

        private readonly float _jumpHeight;

        public JumpPerformer(CharacterController characterController, GravityApplier gravityApplier
            , IInputService inputService, CharacterConfigSO config)
        {
            _characterController = characterController;
            _gravityApplier = gravityApplier;
            _inputService = inputService;
            
            _jumpHeight = config.JumpHeight;
        }

        public void PerformJump()
        {
            if (!_characterController.isGrounded || !_inputService.PlayerJumpHeld())
                return;

            var newVelocity = Mathf.Sqrt(_jumpHeight * Constants.GroundingForce * Constants.Gravity);
            _gravityApplier.SetVerticalVelocity(newVelocity);
        }
    }
}