using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ExtensionMethods;

// Without any "virtual buttons" nonsense.
public class ButtonMenuBase : MonoBehaviour
{
    private EventSystem eventSystem;
    private LayoutGroup group;

    private bool checkIfMenuIsFocused = true;
    
    void Awake(){
        group = GetComponentInChildren<LayoutGroup>(includeInactive: true);
        if (group == null) { Debug.Log("Did not find a VerticalLayoutGroup in children."); }
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        checkIfMenuIsFocused = true;
    }

    public void ResetCursor(){
        SetCursorToFirstButton();
        if (IsThisMenuFocused()) {
            checkIfMenuIsFocused = false;
        }
    }

    public void UpdateFixed()
    {
        if (checkIfMenuIsFocused) {
            ResetCursor();
        }
    }

    private bool IsThisMenuFocused() {
        return GetComponentsInChildren<ButtonUI>().Any(b => b.selected == true);
    }

    private void SetCursorToFirstButton(){
        Button firstBtn = GetButtons().FirstOrDefault();
        if(firstBtn){
            eventSystem.SetSelectedGameObject(firstBtn.gameObject);
        } else {
            Debug.LogWarning("No first button found!");
        }
    }

    public ButtonUI[] GetButtonsUI() => group.GetComponentsInChildren<ButtonUI>();
    public Button[] GetButtons() => group.GetComponentsInChildren<Button>();
}
