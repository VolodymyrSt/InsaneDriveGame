using _Project.Code.Configs.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Services.Audio
{
    public class MusicBuilder : SoundBuilder
    {
        public MusicBuilder(AudioService audioService, CompositionsHolderSO audioClipHolder ) :
            base(audioService, audioClipHolder) { }
        
        public override SoundEmitter Play(string clipId)
        {
            var musicData = CreateMusicData(clipId);
            
            if (!AudioService.CanPlaySound(musicData)) return null;

            var soundEmitter = AudioService.Get();
            soundEmitter.InitSound(musicData);
            
            soundEmitter.transform.position = Position;
            
            if (Parent != null)
                soundEmitter.transform.SetParent(Parent, false);

            if (RandomPitch)
                soundEmitter.WithRandomPitch();
            
            if (IsDelayed)
                soundEmitter.PlayWithDelay(Delay);
            else
                soundEmitter.Play();
            
            return soundEmitter;
        }
        
        private MusicData CreateMusicData(string clipId)
        {
            var clipData = CompositionsHolder.GetMusicById(clipId);
            return new MusicData(SoundType.MUSIC,
                clipData.Clips, clipData.Mixer, Loop,
                PlayOnAwake, IsFrequent,
                Mute, Priority, Volume,
                Pitch, MinDistance, MaxDistance);
        }
    }
}