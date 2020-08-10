using UnityEngine;
using System.Collections;

// For menus, etc.
[RequireComponent(typeof(AudioSource))]
public class AudioMultiSource : MonoBehaviour
{
    public AudioClip soundYes;
    public AudioClip soundNo;
    public AudioClip soundMove;

    private AudioSource audioSource;

    public void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayYesSound()
    {
        audioSource.clip = soundYes;
        audioSource.Play();
    }

    public void PlayNoSound()
    {
        audioSource.clip = soundNo;
        audioSource.Play();
    }

    public void PlayMoveSound()
    {
        audioSource.clip = soundMove;
        audioSource.Play();
    }
}
