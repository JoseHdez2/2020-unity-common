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
    protected EventSystem eventSystem;
    private LayoutGroup layoutGroup;

    private bool checkIfMenuIsFocused = true;

    void Awake(){
        layoutGroup = GetComponentInChildren<LayoutGroup>(includeInactive: true);
        if (layoutGroup == null) { Debug.Log("Did not find a VerticalLayoutGroup in children."); }
        eventSystem = FindObjectOfType<EventSystem>();
    }

    protected void OnEnable(){
        foreach (var button in GetButtonsAll())
        {
            button.gameObject.SetActive(true);
        }
        checkIfMenuIsFocused = true;
    }

    protected void OnDisable() {
        foreach (var button in GetButtons())
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ResetCursor() {
        Debug.Log("ResetCursor");
        SetCursorToFirstButton();
        if (IsThisMenuFocused()) {
            checkIfMenuIsFocused = false;
        }
    }

    public void UpdateFixed() {
        if (checkIfMenuIsFocused) {
            ResetCursor();
        }
    }

    private bool IsThisMenuFocused() {
        return GetComponentsInChildren<ButtonUI>().Any(b => b.selected == true);
    }

    protected void SetCursorToFirstButton(){
        Button firstBtn = GetButtons().FirstOrDefault();
        Debug.Log($"SetCursorToFirstButton: {firstBtn}");
        if(firstBtn){
            eventSystem.SetSelectedGameObject(firstBtn.gameObject);
        } else {
            Debug.LogWarning("No first button found!");
        }
    }

    public ButtonUI[] GetButtonsUI() => layoutGroup.GetComponentsInChildren<ButtonUI>();
    public Button[] GetButtons() => layoutGroup.GetComponentsInChildren<Button>();
    public Button[] GetButtonsAll() => layoutGroup.GetComponentsInChildren<Button>(includeInactive: true);
}
