using _Project.Code.Configs.Character;
using _Project.Code.Core.Services.Input;
using _Project.Code.GamePlay.Camera;
using _Project.Code.Util;
using UnityEngine;

namespace _Project.Code.GamePlay.Character
{
    public class CharacterMover
    {
        private readonly CharacterController _characterController;
        private readonly IInputService _inputService;
        private readonly ICamera _camera;
        private readonly float _speed;

        public CharacterMover(CharacterController characterController, IInputService inputService
            ,ICamera camera, CharacterConfigSO config)
        {
            _characterController = characterController;
            _inputService = inputService;
            _camera = camera;
            
            _speed = config.Speed;
        }

        public void Move()
        {
            var input = _inputService.GetPlayerMoveVector().normalized;
            if (input.sqrMagnitude < Constants.MoveEllipse)
                return;
            
            var direction = _camera.GetNormalizedForward() * input.y + _camera.GetNormalizedRight() * input.x;
            _characterController.Move(direction * _speed * Time.deltaTime);
        }
    }
}