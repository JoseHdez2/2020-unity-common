using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

/// <summary>
///  Change music when this is inside camera bounds. Note: OnBecameVisible requires a Renderer component.
/// </summary>
public class ChangeMusicOnVisible : MonoBehaviour {
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip newMusic;

    void OnBecameVisible() {
        musicSource.Pause();
        if(musicSource.clip.name == newMusic.name){
            musicSource.UnPause();
        }
        if(newMusic){
            musicSource.loop = true;
            musicSource.Play(newMusic);
        }
    }
}
