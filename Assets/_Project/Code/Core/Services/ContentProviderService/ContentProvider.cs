using System;
using _Project.Code.GamePlay.CameraLogic;
using _Project.Code.GamePlay.Car;
using _Project.Code.GamePlay.Character;

namespace _Project.Code.Core.Services.ContentProviderService
{
    public class ContentProvider : IContentProvider
    {
        private ICamera _camera;
        private ICar _car;
        private ICharacter _character;
        
        public ICamera Camera =>  _camera ?? throw new Exception("Can`t provide Camera");
        public ICar Car => _car ?? throw new Exception("Can`t provide Car");
        public ICharacter Character => _character ?? throw new Exception("Can`t provide Character");
        
        public void SetCamera(ICamera camera) => _camera = camera;
        public void SetCar(ICar car) => _car = car;
        public void SetCharacter(ICharacter character) => _character = character;
    }
}