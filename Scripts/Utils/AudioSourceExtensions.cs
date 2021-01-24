using UnityEngine;

namespace ExtensionMethods
{
    public static class AudioSourceExtensions
    {
        public static void Play(this AudioSource source, AudioClip sound){
            source.clip = sound;
            source.Play();
        }

        public static void Toggle(this AudioSource music){   
            if (music.isPlaying) {
                music.Pause();
            } else {
                music.Play();
            }
        }
    }
}