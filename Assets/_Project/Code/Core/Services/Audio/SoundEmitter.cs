using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.Code.Util;

namespace _Project.Code.Core.Services.Audio
{
    public class SoundEmitter : MonoBehaviour
    {
        public SoundData SoundData { get; private set; }
        
        private AudioService _audioService;
        private AudioSource _audioSource;

        private Coroutine _playingCoroutine;
        private float _baseClipVolume;
        
        private Queue<AudioClip> _shuffledTracks = new Queue<AudioClip>();
        private List<AudioClip> _musicTracks;
        private bool _isMusicTrack = false;
        private bool _isMusicTracksLooping = false;
        
        private bool _isPaused = false;

        public void InitEmitter(AudioService audioService)
        {
            _audioService = audioService;
            _audioSource = gameObject.AddOrGet<AudioSource>();
        }

        public void InitSound(SFXData sfxData)
        {
            SoundData = sfxData;
            _audioSource.loop = sfxData.Properties.Loop;
            _audioSource.clip = sfxData.Clip;
            _audioSource.enabled = true;
            InitSoundProperties(sfxData);
            AdjustVolume();
        }

        public void InitSound(MusicData musicData)
        {
            SoundData = musicData;
            _isMusicTrack = true;
            _isMusicTracksLooping = musicData.Properties.Loop;
            PrepareMusicTrack(musicData.Clips);
            InitSoundProperties(musicData);
            AdjustVolume();
        }
        
        public void Play()
        {
            if (_playingCoroutine != null) StopCoroutine(_playingCoroutine);

            if (_isMusicTrack)
                _playingCoroutine = StartCoroutine(PerformMusicTrack());
            else
            {
                _audioSource.Play();
                _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
            }
        }
        
        public void PlayWithDelay(float delay)
        {
            if (_playingCoroutine != null) StopCoroutine(_playingCoroutine);
            
            if (_isMusicTrack)
                _playingCoroutine = StartCoroutine(PerformMusicTrack(delay));
            else
            {
                _audioSource.PlayDelayed(delay);
                _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
            }
        }

        public void Stop()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _playingCoroutine = null;
            }

            _audioSource.Stop();
            _audioService.ReturnToPool(this);
        }
        
        public void Pause()
        {
            _isPaused = true;
            
            if (_isMusicTrack)
            {
                _audioSource.Pause(); 
                return;
            }
            
            if (_playingCoroutine != null) 
                StopCoroutine(_playingCoroutine);
            
            _audioSource.Pause();
        }
        
        public void Resume()
        {
            _isPaused = false;
            _audioSource.UnPause();
            
            if (_isMusicTrack) return;
            
            if (!_audioSource.loop) 
                _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        public void WithRandomPitch(float min = -0.05f, float max = 0.05f) => 
            _audioSource.pitch = SoundData.Properties.Pitch + Random.Range(min, max);

        public void ChangeVolume(float volume) => 
            _audioSource.volume = Mathf.Clamp01(volume * _baseClipVolume);

        private void AdjustVolume()
        {
            _audioSource.volume = SoundData.SoundType == SoundType.SFX ?
                Mathf.Clamp01(_audioService.CurrentSfxVolume * _baseClipVolume) 
                : Mathf.Clamp01(_audioService.CurrentMusicVolume * _baseClipVolume);
        }
        
        private IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            _audioService.ReturnToPool(this);
        }
        
        private IEnumerator PerformMusicTrack(float delay = 0)
        {
            if (_shuffledTracks.Count <= 0) yield break;

            if (delay > 0)
            {
                PlayDelayedRandomMusicTrack(delay);
                yield return new WaitUntil(() => !_isPaused && !_audioSource.isPlaying);
            }
            
            while (true)
            {
                if (_shuffledTracks.Count == 0)
                {
                    if (_isMusicTracksLooping)
                        ShuffleMusicTrackClips();
                    else
                        break;
                }

                PlayRandomMusicTrack();

                yield return new WaitUntil(() => !_isPaused && !_audioSource.isPlaying);
            }

            _audioService.ReturnToPool(this);
        }

        private void PlayDelayedRandomMusicTrack(float delay)
        {
            var clip = _shuffledTracks.Dequeue();
            _audioSource.clip = clip;
            _audioSource.PlayDelayed(delay);
        }
        
        private void PlayRandomMusicTrack()
        {
            var clip = _shuffledTracks.Dequeue();
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        private void PrepareMusicTrack(AudioClip[] clips)
        {
            _shuffledTracks = new Queue<AudioClip>();
            _musicTracks = new List<AudioClip>(clips);

            ShuffleMusicTrackClips();
        }

        private void ShuffleMusicTrackClips()
        {
            var list = new List<AudioClip>(_musicTracks);
            
            for (var i = 0; i < list.Count; i++)
            {
                var randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }

            _shuffledTracks.Clear();
            
            foreach (var clip in list)
                _shuffledTracks.Enqueue(clip);
        }
        
        private void InitSoundProperties(SoundData soundData)
        {
            _audioSource.outputAudioMixerGroup = soundData.Properties.Mixer;
            _audioSource.playOnAwake = soundData.Properties.PlayOnAwake;
            _audioSource.mute = soundData.Properties.Mute;
            _audioSource.priority = soundData.Properties.Priority;
            _audioSource.volume = soundData.Properties.Volume;
            _audioSource.pitch = soundData.Properties.Pitch;
            _audioSource.minDistance = soundData.Properties.MinDistance;
            _audioSource.maxDistance = soundData.Properties.MaxDistance;
            _baseClipVolume = soundData.Properties.Volume;
        }
        
        private void OnDestroy()
        {
            if (_playingCoroutine != null)
                StopCoroutine(_playingCoroutine);
        }
    }
}
