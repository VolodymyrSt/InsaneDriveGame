using _Project.Code.GamePlay.CameraLogic;
using _Project.Code.GamePlay.Car;
using _Project.Code.GamePlay.Character;

namespace _Project.Code.Core.Services.ContentProviderService
{
    public interface IContentProvider
    {
        ICamera Camera { get; }
        ICar Car { get; }
        ICharacter Character { get; }
        void SetCamera(ICamera camera);
        void SetCar(ICar car);
        void SetCharacter(ICharacter character);
    }
}