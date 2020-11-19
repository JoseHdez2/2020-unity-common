using System;
using System.Linq;
using ExtensionMethods;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SrpgMenuAttackType : SrpgMenuBase {
    
    [SerializeField] private Button pfButton;

    public void Open(SrpgUnit unit){
        buttonContainer.ToggleWipe(true);
        srpgController.ToggleFieldCursorFalse();
        gameObject.SetActive(true);
        RefreshButtons(unit);
        audioSource.PlaySound(ESRPGSound.SelectUnit);
        ResetCursor();
        selectedUnit = unit;
    }

    
    public void RefreshButtons(SrpgUnit unit){
        DestroyButtons();
        unit.items.ToList().ForEach(b => AddButton(b));
        AddCancelButton();
    }

    public void AddButton(SrpgItem item) {
        Button b = Instantiate(pfButton, buttonContainer.transform);
        b.onClick.AddListener(() => SelectAttackType(item));
        b.GetComponentInChildren<TMP_Text>().text = $"{item.typeId} ({item.remainingDurability})";
        b.interactable = true;
    }

    private void AddCancelButton(){
        Button b = Instantiate(pfButton, buttonContainer.transform);
        b.onClick.AddListener(() => HandleCancel());
        b.GetComponentInChildren<TMP_Text>().text = "Cancel";
        b.interactable = true;
    }

    public void SelectAttackType(SrpgItem item){
        Debug.Log(item);
    }

    public void DestroyButtons(){
        buttonContainer.transform.DeleteAllChildren();
    }

    protected override void HandleCancel(){
        audioSource.PlaySound(ESRPGSound.Buzzer);
        // throw new NotImplementedException();
    }
}