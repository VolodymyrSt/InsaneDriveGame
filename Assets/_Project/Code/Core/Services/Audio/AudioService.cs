using System;
using System.Collections.Generic;
using _Project.Code.Configs.Audio;
using _Project.Code.Core.Factory;
using _Project.Code.Core.Factory.Pool;
using _Project.Code.Core.Services.StaticData;
using _Project.Code.Util;
using UnityEngine;
using UnityEngine.Pool;
using VContainer.Unity;

namespace _Project.Code.Core.Services.Audio
{
    public class AudioService : IAudioService, IInitializable
    {
        private const int DEFAULT_CAPACITY = 10;
        private const int MAX_POOL_SIZE = 100;
        private const int MAX_SOUND_INSTANCES = 30;

        private IObjectPool<SoundEmitter> _pooledSoundEmitters;
        
        private readonly List<SoundEmitter> _activeSoundEmitters;
        private readonly List<SoundEmitter> _inactiveSoundEmitters;
        private readonly Queue<SoundEmitter> _frequentSoundEmitters;

        private readonly IGameObjectBuilderFactory _gameObjectBuilderFactory;
        private readonly CompositionsHolderSO _compositionsHolder;
        private SoundHolder _soundHolder;

        private readonly bool _collectionCheck = true;
        
        private float _currentSfxVolume = 1f;
        private float _currentMusicVolume = 1f;
        public float CurrentSfxVolume => _currentSfxVolume;
        public float CurrentMusicVolume => _currentMusicVolume;
        
        public SoundHolder Holder => _soundHolder ??= 
               _gameObjectBuilderFactory.BuildNewFor<SoundHolder>("SoundHolder").AsPersist().Create();

        public AudioService(IStaticDataService staticDataService, IGameObjectBuilderFactory gameObjectBuilderFactory)
        {
            _compositionsHolder = staticDataService.Compositions;
            _gameObjectBuilderFactory = gameObjectBuilderFactory;
            
            _activeSoundEmitters = new List<SoundEmitter>();
            _inactiveSoundEmitters = new List<SoundEmitter>();
            _frequentSoundEmitters = new Queue<SoundEmitter>();
        }
        
        public void Initialize() => InitializePool();

        public SoundBuilder BuildSFX() => new SFXBuilder(this, _compositionsHolder);
        public SoundBuilder BuildMusic() => new MusicBuilder(this, _compositionsHolder);

        public SoundEmitter Get() => _pooledSoundEmitters.Get();
        
        public void ReturnToPool(SoundEmitter soundEmitter)
        {
            if (_inactiveSoundEmitters.Contains(soundEmitter)) 
                throw new Exception($"You are trying to return {nameof(soundEmitter)} that is already inactive");
            
            _pooledSoundEmitters.Release(soundEmitter);
            Holder.ReturnToPlace(soundEmitter);
        }

        public void PauseAllSoundsByType(SoundType soundType)
        {
            if (_activeSoundEmitters.Count <= 0) return;

            for (var i = _activeSoundEmitters.Count - 1; i >= 0; i--)
                if (_activeSoundEmitters[i].SoundData.SoundType == soundType)
                    _activeSoundEmitters[i].Pause();
        }
        
        public void ResumeAllSoundsByType(SoundType soundType)
        {
            if (_activeSoundEmitters.Count <= 0) return;

            for (var i = _activeSoundEmitters.Count - 1; i >= 0; i--)
                if (_activeSoundEmitters[i].SoundData.SoundType == soundType)
                    _activeSoundEmitters[i].Resume();
        }
        
        public void ChangeSfxVolume(float volume)
        {
            _currentSfxVolume = volume;
            ChangeVolumeForSoundsByType(volume, SoundType.SFX);
        }
        
        public void ChangeMusicVolume(float volume)
        {
            _currentMusicVolume = volume;
            ChangeVolumeForSoundsByType(volume, SoundType.MUSIC);
        }

        public bool CanPlaySound(SoundData soundData)
        {
            if (!soundData.Properties.FrequentSound) return true;

            if (_frequentSoundEmitters.Count >= MAX_SOUND_INSTANCES && _frequentSoundEmitters.TryDequeue(out var soundEmitter))
            {
                try {
                    soundEmitter.Stop();
                    return true;
                }
                catch {
                    Debug.Log("SoundEmitter was already released");
                }
                return false;
            }
            
            return true;
        }
        
        public void RegisterFrequentSoundEmitter(SoundEmitter soundEmitter) =>
            _frequentSoundEmitters.Enqueue(soundEmitter);

        private void InitializePool()
        {
            _pooledSoundEmitters = new ObjectPool<SoundEmitter>(
                OnCreatePoolObject,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                _collectionCheck,
                DEFAULT_CAPACITY,
                MAX_POOL_SIZE);

            PreloadPoolObject(DEFAULT_CAPACITY);
        }

        private void PreloadPoolObject(int initialCapacity)
        {
            var preloadedEmitters = new SoundEmitter[initialCapacity];
            
            for (int i = 0; i < initialCapacity; i++)
                preloadedEmitters[i] = _pooledSoundEmitters.Get();

            foreach (var preloadedEmitter in preloadedEmitters)
                _pooledSoundEmitters.Release(preloadedEmitter);
        }
        
        private SoundEmitter OnCreatePoolObject()
        {
            var soundEmitter = _gameObjectBuilderFactory
                .BuildNewFor<SoundEmitter>("SoundEmitter")
                .With<AudioSource>()
                .Under(Holder.GetPlace())
                .Create();
            
            soundEmitter.InitEmitter(this);
            soundEmitter.SetActive(false);
            _inactiveSoundEmitters.Add(soundEmitter);
            return soundEmitter;
        }

        private void OnTakeFromPool(SoundEmitter soundEmitter)
        {
            soundEmitter.SetActive(true);
            _inactiveSoundEmitters.Remove(soundEmitter);
            _activeSoundEmitters.Add(soundEmitter);
        }

        private void OnReturnedToPool(SoundEmitter soundEmitter)
        {
            soundEmitter.SetActive(false);
            _inactiveSoundEmitters.Add(soundEmitter);
            _activeSoundEmitters.Remove(soundEmitter);
        }

        private void OnDestroyPoolObject(SoundEmitter soundEmitter)
        {
            if (_activeSoundEmitters.Contains(soundEmitter))
                _activeSoundEmitters.Remove(soundEmitter);

            if (_inactiveSoundEmitters.Contains(soundEmitter))
                _inactiveSoundEmitters.Remove(soundEmitter);
        }
        
        private void ChangeVolumeForSoundsByType(float volume, SoundType soundType)
        {
            foreach (var emitter in _activeSoundEmitters)
                if (emitter != null && emitter.SoundData.SoundType == soundType)
                    emitter.ChangeVolume(volume);
        }
    }
}
