using System;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Code.Core.Services.Audio
{
    [Serializable]
    public class SoundData 
    {
        public SoundType SoundType;
        public SoundDataProperties Properties;

        public SoundData(SoundType soundType, AudioMixerGroup mixer, bool loop, bool playOnAwake
            , bool frequentSound, bool mute, int priority, float volume,
            float pitch, float minDistance, float maxDistance)
        {
            SoundType = soundType;
            Properties = new SoundDataProperties(mixer, loop, playOnAwake,
                frequentSound, mute, priority, volume, pitch,  minDistance, maxDistance);
        }
    }

    [Serializable]
    public class SFXData : SoundData
    {
        public AudioClip Clip;
        
        public SFXData(SoundType soundType, AudioClip clip,
            AudioMixerGroup mixer, bool loop, bool playOnAwake,
            bool frequentSound, bool mute, int priority, 
            float volume, float pitch, float minDistance,
            float maxDistance) : base(soundType, mixer, loop, playOnAwake, 
            frequentSound, mute, priority, volume,
            pitch, minDistance, maxDistance)
        {
            Clip = clip;
        }
    }

    [Serializable]
    public class MusicData : SoundData
    {
        public AudioClip[] Clips;

        public MusicData(SoundType soundType, AudioClip[] clips, 
            AudioMixerGroup mixer, bool loop, bool playOnAwake,
            bool frequentSound, bool mute, int priority, 
            float volume, float pitch, float minDistance,
            float maxDistance) : base(soundType, mixer, loop, playOnAwake, 
            frequentSound, mute, priority, volume,
            pitch, minDistance, maxDistance)
        {
            Clips = clips;
        }
    }

    [Serializable]
    public class SoundDataProperties
    {
        public AudioMixerGroup Mixer;
        public bool Loop;
        public bool PlayOnAwake;
        public bool FrequentSound;

        public bool Mute = false;
        public int Priority = 128;
        public float Volume = 1f;
        public float Pitch = 1f;

        public float MinDistance = 1f;
        public float MaxDistance = 500f;
        
        public SoundDataProperties(AudioMixerGroup mixer, bool loop, bool playOnAwake
            , bool frequentSound, bool mute, int priority, float volume,
            float pitch, float minDistance, float maxDistance)
        {
            Mixer = mixer;
            Loop = loop;
            PlayOnAwake = playOnAwake;
            FrequentSound = frequentSound;

            Mute = mute;
            Priority = priority;
            Volume = volume;
            Pitch = pitch;
            MinDistance = minDistance;
            MaxDistance = maxDistance;
        }
    }
}
