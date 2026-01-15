using _Project.Code.Core.Services.Input;
using _Project.Code.GamePlay.CameraLogic.Factory;
using _Project.Code.GamePlay.Car.Factory;
using _Project.Code.GamePlay.Character.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Infrastructure.StateMachine.States
{
    public class EnterLevelState : IEnterableState
    {
        private readonly CharacterFactory _characterFactory;
        private readonly CameraFactory _cameraFactory;
        private readonly CarFactory _carFactory;
        private readonly IInputService _inputService;

        public EnterLevelState(CharacterFactory characterFactory, CameraFactory cameraFactory,
        CarFactory carFactory,  IInputService inputService)
        {
            _characterFactory = characterFactory;
            _cameraFactory = cameraFactory;
            _carFactory = carFactory;
            _inputService = inputService;
        }
        
        public async UniTask Enter()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _inputService.Enable(true);
            
             var character = await _characterFactory.CreateCharacter(Vector3.up * 2);
             var camera = await _cameraFactory.CreateCamera();
             var car = await _carFactory.CreateCar(Vector3.up * 10f + Vector3.left * 5f);
             
             character.Init(camera);
             camera.Init(character);
             car.Init();
        }
    }
}