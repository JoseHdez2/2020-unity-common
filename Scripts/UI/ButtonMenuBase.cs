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
    
    private CanvasGroup canvasGroup;

    private bool checkIfMenuIsFocused = true;

    [Tooltip("Optional ImageWipe container. If present, it will wipe the menu on and off the screen.")]
    [SerializeField] protected ImageWipe buttonContainer;
    
    void Awake(){
        canvasGroup = GetComponentInChildren<CanvasGroup>(includeInactive: true);
        if (canvasGroup == null) { Debug.Log("Did not find a CanvasGroup in children."); }
        layoutGroup = GetComponentInChildren<LayoutGroup>(includeInactive: true);
        if (layoutGroup == null) { Debug.Log("Did not find a LayoutGroup in children."); }
        eventSystem = FindObjectOfType<EventSystem>();
        canvasGroup.blocksRaycasts = false; // to override OnEnable setting it to 'true'.
        if(buttonContainer){   
            buttonContainer.ToggleWipe(false);
        }
    }

    protected void OnEnable()
    {
        foreach (var button in GetButtonsAll())
        {
            button.gameObject.SetActive(true);
        }
        checkIfMenuIsFocused = true;
        canvasGroup.blocksRaycasts = true; // TODO optimally, it should wait for the end of an animation or something, before doing this.
    }

    protected void OnDisable()
    {
        foreach (var button in GetButtons())
        {
            button.gameObject.SetActive(false);
        }
        canvasGroup.blocksRaycasts = false;
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

    protected void SetCursorToFirstButton(){
        Button firstBtn = GetButtons().FirstOrDefault();
        if(firstBtn){
            eventSystem.SetSelectedGameObject(firstBtn.gameObject);
        } else {
            Debug.LogWarning("No first button found!");
        }
    }

    public void Close(){
        StartCoroutine(CrClose());
    }

    protected IEnumerator CrClose(){
        buttonContainer.ToggleWipe(false);
        yield return new WaitUntil(() => buttonContainer.isDone());
        gameObject.SetActive(false);
    }

    public ButtonUI[] GetButtonsUI() => layoutGroup.GetComponentsInChildren<ButtonUI>();
    public Button[] GetButtons() => layoutGroup.GetComponentsInChildren<Button>();
    public Button[] GetButtonsAll() => layoutGroup.GetComponentsInChildren<Button>(includeInactive: true);
}
