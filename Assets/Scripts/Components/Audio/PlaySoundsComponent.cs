using System;
using UnityEngine;

namespace Components.Audio
{
    public class PlaySoundsComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioData[] _sounds;

        public void Play(string id)
        {
            foreach (var audioData in _sounds)
            {
                if (audioData.Id != id) continue;
                _source.PlayOneShot(audioData.Clip);
                break;
            }
        }
        
        [Serializable]
        public class AudioData
        {
            [SerializeField] private string id;
            [SerializeField] private AudioClip clip;

            public string Id => id;
            public AudioClip Clip => clip;
        }
    }
}