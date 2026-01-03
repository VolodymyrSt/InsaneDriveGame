using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Code.Configs.Audio
{
    [Serializable]
    public class AudioClipData
    {
        [HorizontalGroup("Audio", width: 0.3f)]
        [LabelWidth(20)]
        public string ID;
        [HorizontalGroup("Audio", width: 0.5f)]
        [LabelWidth(25)]
        public AudioClip Clip;
        [HorizontalGroup("Audio")]
        [LabelWidth(40)]
        public AudioMixerGroup Mixer;

        public AudioClipData(string id, AudioClip clip, AudioMixerGroup mixer)
        {
            ID = id;
            Clip = clip;
            Mixer = mixer;
        }
    }
    
    [Serializable]
    public class AudioClipGroupData
    {
        [HorizontalGroup("Audio", width: 0.3f), LabelWidth(20)]
        public string ID;
        [HorizontalGroup("Audio", width: 0.5f), LabelWidth(25)]
        public AudioClip[] Clips;
        [HorizontalGroup("Audio"), LabelWidth(40)]
        public AudioMixerGroup Mixer;

        public AudioClipData GetRandom()
        {
            if (Clips == null || Clips.Length == 0)
                throw new Exception($"AudioClipGroupData -> Clips is not initialized");
            
            var randomIndex = UnityEngine.Random.Range(0, Clips.Length);
            var clip = Clips[randomIndex];
            
            return new AudioClipData(ID, clip, Mixer);
        }
    }
}