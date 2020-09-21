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

[Serializable]
public class ButtonData {
    [SerializeField] public int index;
    [SerializeField] public string name;
    [SerializeField] public Sprite sprite;
    [SerializeField] public bool interactable = true;
    [SerializeField] public UnityEvent action;
}

public class ButtonMenu : MonoBehaviour
{
    /// <summary>Virtual buttons that will be shown as real buttons when this menu is enabled.</summary>
    public List<ButtonData> buttonsData;
    public Button buttonPrefab;

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
        RefreshButtons();
        checkIfMenuIsFocused = true;
    }

    public void RefreshButtons(){
        DestroyButtons();
        buttonsData.ToList().ForEach(b => AddButton(b));
    }

    public void UpdateFixed()
    {
        if (checkIfMenuIsFocused) {
            SetCursorToFirstButton();
            if (IsThisMenuFocused()) {
                checkIfMenuIsFocused = false;
            }
        }
    }

    private bool IsThisMenuFocused() {
        return GetComponentsInChildren<ButtonUI>().Any(b => b.selected == true);
    }

    private void SetCursorToFirstButton(){
        GameObject firstBtn = GetComponentsInChildren<ButtonUI>().First().gameObject;
        eventSystem.SetSelectedGameObject(firstBtn);
    }

    public void AddButtonData(ButtonData buttonData){
        buttonsData.Add(buttonData);
    }

    public void AddButton(ButtonData buttonData) {
        Button b = Instantiate(buttonPrefab, group.transform);
        if (buttonData.action == null) { throw new ArgumentNullException("Cannot have a button with no action!"); }
        b.onClick.AddListener(buttonData.action.Invoke);
        TMP_Text tmpText = b.GetComponentInChildren<TMP_Text>();
        tmpText.text = buttonData.name;
        b.interactable = buttonData.interactable;
    }

    /// <summary>
    /// Destroy the virtual buttons. Useful for redrawing.
    /// </summary>
    public void ClearButtonData()
    {
        buttonsData.Clear();
    }

    /// <summary>
    /// Destroy the real buttons and only keep the virtual ones. Useful for redrawing.
    /// </summary>
    public void DestroyButtons(){
        group.transform.DeleteAllChildren();
    }


    public Button[] GetButtons() => group.GetComponentsInChildren<Button>();
}
