using _Project.Code.Configs.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Services.Audio
{
    public abstract class SoundBuilder
    {
        protected readonly CompositionsHolderSO CompositionsHolder;
        protected readonly AudioService AudioService;

        protected Transform Parent = null;
        protected Vector3 Position = Vector3.zero;
        protected bool RandomPitch = false;
        protected bool IsDelayed = false;
        
        protected bool Loop = false;
        protected bool PlayOnAwake = false;
        protected bool IsFrequent = false;

        protected bool Mute = false;
        protected int Priority = 128;
        protected float Volume = 1f;
        protected float Pitch = 1f;

        protected float MinDistance = 1f;
        protected float MaxDistance = 500f;
        protected float Delay = 0f;

        protected SoundBuilder(AudioService audioService, CompositionsHolderSO audioClipHolder)
        {
            AudioService = audioService;
            CompositionsHolder = audioClipHolder;
        }
        
        public abstract SoundEmitter Play(string clipId);
        
        public SoundBuilder WithPosition(Vector3 position)
        {
            Position = position;
            return this;
        }
        
        public SoundBuilder WithCameraPosition()
        {
            if (Camera.main == null)
            {
                Debug.LogWarning("Camera not found for sound builder");
                return this;
            }
                
            Position = Camera.main.transform.position;
            return this;
        }
        
        public SoundBuilder WithParent(Transform parent)
        {
            Parent = parent;
            return this;
        }
        
        public SoundBuilder WithRandomPitch()
        {
            RandomPitch = true;
            return this;
        }
        
        public SoundBuilder WithLoop()
        {
            Loop = true;
            return this;
        }
        
        public SoundBuilder WithDelay(float delay)
        {
            Delay = delay;
            IsDelayed = true;
            return this;
        }
        
        public SoundBuilder WithPlayOnAwake()
        {
            PlayOnAwake = true;
            return this;
        }
        
        public SoundBuilder WithMute()
        {
            Mute = true;
            return this;
        }
        
        public SoundBuilder WithPriority(int priority)
        {
            Priority = priority;
            return this;
        }
        
        public SoundBuilder WithMinDistance(float minDistance)
        {
            MinDistance = minDistance;
            return this;
        }
        
        public SoundBuilder WithMaxDistance(float maxDistance)
        {
            MaxDistance = maxDistance;
            return this;
        }
        
        public SoundBuilder WithVolume(float volume)
        {
            Volume = volume;
            return this;
        }
        
        public SoundBuilder WithPitch(float pith)
        {
            Pitch = pith;
            return this;
        }
    }
}
