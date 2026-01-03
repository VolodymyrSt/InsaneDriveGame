using _Project.Code.Util;
using UnityEngine;

namespace _Project.Code.GamePlay.Character
{
    public class GravityApplier
    {
        private readonly CharacterController _characterController;
        private float _verticalVelocity;
        
        public GravityApplier(CharacterController characterController) => 
            _characterController = characterController;

        public void ApplyGravity()
        {
            if (_characterController.isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = Constants.GroundingForce;

            _verticalVelocity += Constants.Gravity * Time.deltaTime;
            _characterController.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
        }

        public void SetVerticalVelocity(float verticalVelocity) =>
            _verticalVelocity = verticalVelocity;
    }
}