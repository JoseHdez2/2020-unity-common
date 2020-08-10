using UnityEngine;
using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioSourceMultiBase<TEnum> : MonoBehaviour
{
    public SerializableDictionaryBase<TEnum, AudioClip> soundDict;
    private AudioSource audioSource;

    public void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(TEnum sound){
        audioSource.clip = soundDict[sound];
        audioSource.Play();
    }
}
