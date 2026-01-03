using _Project.Code.Configs.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Services.Audio
{
    public class SFXBuilder : SoundBuilder
    {
        public SFXBuilder(AudioService audioService, CompositionsHolderSO audioClipHolder) : 
            base(audioService, audioClipHolder) { }
        
        public SoundBuilder WithFrequence()
        {
            IsFrequent = true;
            return this;
        }
        
        public override SoundEmitter Play(string clipId)
        {
            var soundData = CreateSoundData(clipId);
            
            if (!AudioService.CanPlaySound(soundData)) return null;

            var soundEmitter = AudioService.Get();
            soundEmitter.InitSound(soundData);
            
            soundEmitter.transform.position = Position;
            
            if (Parent != null)
                soundEmitter.transform.SetParent(Parent, false);

            if (RandomPitch)
                soundEmitter.WithRandomPitch();

            if (IsFrequent)
                AudioService.RegisterFrequentSoundEmitter(soundEmitter);

            if (IsDelayed)
                soundEmitter.PlayWithDelay(Delay);
            else
                soundEmitter.Play();
            
            return soundEmitter;
        }
        
        private SFXData CreateSoundData(string clipId)
        {
            var clipData = CompositionsHolder.GetSFXById(clipId);
            return new SFXData(SoundType.SFX,
                clipData.Clip, clipData.Mixer, Loop,
                PlayOnAwake, IsFrequent,
                Mute, Priority, Volume,
                Pitch, MinDistance, MaxDistance);
        }
    }
}