using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class SimplePauseManager : ToggleWithKey
{
    [SerializeField] AudioSource music;
    private bool isPaused = false;

    new public void ToggleObject()
    {
        base.ToggleObject();
        music.Toggle();
        isPaused = !isPaused;
        Debug.Log($"Set to {(isPaused ? "paused" : "unpaused")}!");
        Time.timeScale = isPaused ? 0 : 1;
    }
}
