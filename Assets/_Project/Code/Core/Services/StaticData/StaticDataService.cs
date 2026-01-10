using _Project.Code.Configs.Audio;
using _Project.Code.Configs.Camera;
using _Project.Code.Configs.Character;
using _Project.Code.Core.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;
        private CharacterConfigSO _characterConfig;
        private CompositionsHolderSO _compositions;
        private CameraConfigSO _cameraConfig;
        
        public CharacterConfigSO CharacterConfig => _characterConfig;
        public CompositionsHolderSO Compositions => _compositions;
        public CameraConfigSO CameraConfig => _cameraConfig;
        
        public StaticDataService(IAssetProvider assetProvider) =>
            _assetProvider = assetProvider;

        public async UniTask Initialize()
        {
            _characterConfig = await Load<CharacterConfigSO>(AssetsAddress.CharacterConfig);
            _compositions = await Load<CompositionsHolderSO>(AssetsAddress.CompositionsHolder);
            _cameraConfig = await Load<CameraConfigSO>(AssetsAddress.CameraConfig);
        }
        
        private async UniTask<T> Load<T>(string address) where T : ScriptableObject => 
            await _assetProvider.Load<T>(address);
    }
}