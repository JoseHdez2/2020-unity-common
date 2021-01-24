using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePauseManager : ToggleWithKey
{
    [SerializeField] AudioSource music;

    new public void ToggleObject()
    {
        base.ToggleObject();
        ToggleMusic(music);
        
    }

    private void ToggleMusic(AudioSource music) {
    }
}
