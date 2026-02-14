using _Project.Code.Configs.Audio;
using _Project.Code.Configs.Camera;
using _Project.Code.Configs.Car;
using _Project.Code.Configs.Character;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Core.Services.StaticData
{
    public interface IStaticDataService
    {
        CharacterConfigSO CharacterConfig { get; }
        CompositionsHolderSO Compositions { get; }
        CameraConfigSO CameraConfig { get; }
        CarConfigSO CarConfig { get; }
        UniTask Initialize();
    }
}