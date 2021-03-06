﻿using UnityEngine;
using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioSourceMultiBase<TEnum> : MonoBehaviour
{
    public SerializableDictionaryBase<TEnum, AudioClip> soundDict;
    [Tooltip("If a sound is already playing, don't play over it.")]
    public bool respectSoundSeniority = false;
    private AudioSource audioSource;

    public void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(TEnum sound){
        if (!audioSource) { Debug.Log("No AudioSource attached(?)."); return; }
        if(respectSoundSeniority && audioSource.isPlaying) {
            Debug.Log("Already playing a sound!");
            return;
        }
        if (soundDict == null){
            Debug.LogError("SoundDict is null!");
            return;
        }
        if (soundDict.Keys.Contains(sound)) {
            audioSource.clip = soundDict[sound];
            audioSource.Play();
        } else {
            Debug.Log($"Cannot play sound '{sound}', which is not in the soundDict.");
        }
    }

    public void Pause(){
        audioSource.Pause();
    }

    public bool IsPlaying(){
        return audioSource.isPlaying;
    }

    public void SetPitch(float pitch) { audioSource.pitch = pitch; }

    public void PlaySoundWithPitch(TEnum sound, float pitch){
        SetPitch(pitch);
        PlaySound(sound);
    }
}
