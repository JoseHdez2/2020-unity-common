using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// For use in toggling stuff with a button (e.g. pause menus).
// Uses new InputSystem.
public class ToggleWithKey : MonoBehaviour
{
    public GameObject[] objsToToggle;
    public Key keyToToggle;
    public string tagToToggle;
    public AudioSource toggleSound;

    void Update()
    {
        if (Keyboard.current[keyToToggle].wasPressedThisFrame) {
            ToggleObject();
        }
    }

    public void ToggleObject() {
        toggleSound.Play();
        objsToToggle.ToList().ForEach(o => o.SetActive(!o.activeSelf));
        if (!string.IsNullOrEmpty(tagToToggle))
        {
            GameObject.FindGameObjectsWithTag(tagToToggle).ToList().ForEach(o => o.SetActive(!o.activeSelf));
        }
    }
}
