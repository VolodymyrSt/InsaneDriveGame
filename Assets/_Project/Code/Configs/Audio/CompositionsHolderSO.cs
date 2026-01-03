using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Code.Configs.Audio
{
    [CreateAssetMenu(fileName = "CompositionsHolder", menuName = "AudioSystem")]
    public class CompositionsHolderSO : ScriptableObject
    {
        public List<SFXCompositionData> SFXCompositions = new();
        [Space(20f)]
        public List<SFXGroupCompositionData> SFXGroupCompositions = new();
        [Space(20f)]
        public MusicCompositionData MusicComposition;

        public AudioClipData GetSFXById(string id)
        {
            foreach (var SFXComposition in SFXCompositions)
            {
                var sfx = SFXComposition.SFXs.Find(x => x.ID == id);
                if (sfx != null)
                    return sfx;
            }

            foreach (var SFXGroupComposition in SFXGroupCompositions)
            {
                var group = SFXGroupComposition.SFXGroups.Find(x => x.ID == id);
                if (group != null)
                    return group.GetRandom();
            }

            throw new Exception($"Couldn't find sfx with id {id}");
        }
        
        public AudioClipGroupData GetMusicById(string id)
        {
            var music = MusicComposition.MusicTracks.Find(x => x.ID == id);

            if (music != null)
                return music;
            
            throw new Exception($"Couldn't find music with id {id}");
        }
    }

    [Serializable]
    public class SFXCompositionData
    {
        public CompositionCategory CompositionCategory;
        public List<AudioClipData> SFXs = new();
    }
    
    [Serializable]
    public class SFXGroupCompositionData
    {
        public CompositionCategory CompositionCategory;
        public List<AudioClipGroupData> SFXGroups = new();
    }
    
    [Serializable]
    public class MusicCompositionData
    {
        public List<AudioClipGroupData> MusicTracks = new();
    }

    public enum CompositionCategory
    {
        None, Player, Machines, Enemy, Nature, UI, Towers, Music, Menu
    }
}
