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
    public bool canPress = true;

    private Keyboard keyboard;
    void Start(){
        keyboard = Keyboard.current;
        if(keyboard == null){
            Debug.LogError("No 'current' Keyboard found!");
        }
    }

    void Update()
    {
        // TODO this 'canPress' check seems suboptimal.
        if (canPress && keyboard != null && keyboard[keyToToggle].wasPressedThisFrame) {
            ToggleObject();
            canPress = false;
        }
        if (keyboard != null && keyboard[keyToToggle].wasReleasedThisFrame) {
            canPress = true;
        }
    }

    public void ToggleObject() {
        if (toggleSound) { toggleSound.Play(); }
        objsToToggle.ToList().ForEach(o => o.SetActive(!o.activeSelf));
        if (!string.IsNullOrEmpty(tagToToggle)){
            FindObjectsOfType<GameObject>(includeInactive: true)
                .Where(o => o.CompareTag(tagToToggle)).ToList()
                .ForEach(o => o.SetActive(!o.activeSelf));
        }
    }
}
