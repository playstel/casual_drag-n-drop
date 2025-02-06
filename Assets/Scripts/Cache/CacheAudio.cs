using System.Linq;
using Configs;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cache
{
    public class CacheAudio : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private ConfigSounds ConfigSounds;
        
        [Header("Current")]
        [SerializeField] private  BaseEnums.Sounds _currentSound;
        
        [Header("Source")]
        [SerializeField] private  AudioSource soundSource;
        
        public void Play(BaseEnums.Sounds sound, bool playOverLast = true, 
            float pitch = 0.05f, bool playWithDupe = false)
        {
            if (!playWithDupe)
            {
                if (soundSource.isPlaying && _currentSound == sound) return;
            }

            if (!playOverLast)
            {
                if (soundSource.isPlaying) return;
            }
            
            _currentSound = sound;
            
            if(pitch > 0) soundSource.pitch = RandomValue(pitch);
            else {soundSource.pitch = 1;}

            var clip = GetAudioClip(sound);

            if (clip) soundSource.PlayOneShot(clip, RandomValue(0.1f));
        }

        public AudioClip GetAudioClip(BaseEnums.Sounds type)
        {
            var clip = ConfigSounds.soundList.FirstOrDefault(i => i.soundType == type);
            return clip?.soundClip;
        }
        
        private float RandomValue(float amplitude)
        {
            return Random.Range(1 - amplitude, 1 + amplitude);
        }
    }
}
