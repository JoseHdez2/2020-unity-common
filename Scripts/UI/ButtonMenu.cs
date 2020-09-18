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
    [SerializeField] public string name;
    [SerializeField] public Sprite sprite;
    [SerializeField] public bool interactable = true;
    public UnityEvent action;
}

public class ButtonMenu : MonoBehaviour
{
    public List<ButtonData> buttonsData;
    public Button buttonPrefab;

    private EventSystem eventSystem;
    private VerticalLayoutGroup group;

    private bool checkIfMenuIsFocused = true;
    
    void Awake(){
        group = GetComponentInChildren<VerticalLayoutGroup>(includeInactive: true);
        if (group == null) { Debug.Log("Did not find a VerticalLayoutGroup in children."); }
        buttonsData.ToList().ForEach(b => AddButton(b));
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        checkIfMenuIsFocused = true;
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
        if(!IsThisMenuFocused()) {
            GameObject firstBtn = GetComponentsInChildren<ButtonUI>().First().gameObject;
            eventSystem.SetSelectedGameObject(firstBtn);
        }
    }
    
    public void AddButton(ButtonData buttonData){
        Button b = Instantiate(buttonPrefab, group.transform);
        b.onClick.AddListener(buttonData.action.Invoke);
        TMP_Text tmpText = b.GetComponentInChildren<TMP_Text>();
        tmpText.text = buttonData.name;
        b.interactable = buttonData.interactable;
    }

    public void ClearButtons(){
        group.transform.DeleteAllChildren();
    }

    public Button[] GetButtons() => group.GetComponentsInChildren<Button>();
}
