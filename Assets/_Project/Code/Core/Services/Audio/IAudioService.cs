namespace _Project.Code.Core.Services.Audio
{
    public interface IAudioService
    {
        float CurrentSfxVolume { get; }
        float CurrentMusicVolume { get; }
        
        SoundBuilder BuildSFX();
        SoundBuilder BuildMusic();
        void ReturnToPool(SoundEmitter soundEmitter);
        void PauseAllSoundsByType(SoundType soundType);
        void ResumeAllSoundsByType(SoundType soundType);
        void ChangeSfxVolume(float volume);
        void ChangeMusicVolume(float volume);
    }
}