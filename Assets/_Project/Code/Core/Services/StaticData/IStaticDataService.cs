using _Project.Code.Configs.Audio;
using _Project.Code.Configs.Character;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Core.Services.StaticData
{
    public interface IStaticDataService
    {
        CharacterConfigSO CharacterConfig { get; }
        CompositionsHolderSO Compositions { get; }
        UniTask Initialize();
    }
}