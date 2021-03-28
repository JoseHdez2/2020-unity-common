using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class SimplePauseManager : ToggleWithKey
{
    [SerializeField] AudioSource music;
    private bool isPaused = false;
    [Header("We must toggle MouseLook to free the cursor.")]
    public string dummy;

    override public void ToggleMoreStuff()
    {
        if(music) { music.Toggle(); }
        isPaused = !isPaused;
        Debug.Log($"Set to {(isPaused ? "paused" : "unpaused")}!");
        Time.timeScale = isPaused ? 0 : 1;
    }
}
