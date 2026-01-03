using _Project.Code.Core.Services.Input;
using _Project.Code.GamePlay.Camera.Factory;
using _Project.Code.GamePlay.Character.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Infrastructure.StateMachine.States
{
    public class EnterLevelState : IEnterableState
    {
        private readonly PlayerFactory _playerFactory;
        private readonly CameraFactory _cameraFactory;
        private readonly IInputService _inputService;

        public EnterLevelState(PlayerFactory playerFactory, CameraFactory cameraFactory
        , IInputService inputService)
        {
            _playerFactory = playerFactory;
            _cameraFactory = cameraFactory;
            _inputService = inputService;
        }
        
        public async UniTask Enter()
        {
            _inputService.Enable(true);
            
             var character = await _playerFactory.CreateCharacter(Vector3.up * 2);
             var camera = await _cameraFactory.CreateCamera();
             
             character.Init(camera);
             camera.Init(character.CameraHolder);
        }
    }
}