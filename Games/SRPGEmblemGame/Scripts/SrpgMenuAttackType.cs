using System;
using System.Linq;
using ExtensionMethods;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SrpgMenuAttackType : SrpgMenuBase {
    
    [SerializeField] private Button pfButton;

    public void Open(SrpgUnit attackingUnit, SrpgUnit targetedUnit){
        buttonContainer.ToggleWipe(true);
        srpgController.ToggleFieldCursorFalse();
        gameObject.SetActive(true);
        RefreshButtons(attackingUnit, targetedUnit);
        audioSource.PlaySound(ESrpgSound.SelectUnit);
        ResetCursor();
        selectedUnit = attackingUnit;
    }
    
    public void RefreshButtons(SrpgUnit attackingUnit, SrpgUnit targetedUnit){
        DestroyButtons();
        attackingUnit.items.ToList().ForEach(item => TryAddButton(attacker: attackingUnit, target: targetedUnit, weapon: item));
        AddCancelButton();
    }

    public void TryAddButton(SrpgUnit attacker, SrpgUnit target, SrpgItem weapon) {
        SrpgAttack attack = new SrpgAttack(attacker: attacker, attackerPos: attacker.transform.position, weapon: weapon, weaponType: srpgController.database.itemTypes[weapon.typeId], target: target);

        if(!attack.IsValid()){
            return;
        }
        Button b = Instantiate(pfButton, buttonContainer.transform);
        b.onClick.AddListener(() => SelectAttackType(attack));
        b.GetComponentInChildren<TMP_Text>().text = $"{attack.weaponType.name} ({weapon.remainingDurability})";
        b.interactable = true;
    }

    private void AddCancelButton(){
        Button b = Instantiate(pfButton, buttonContainer.transform);
        b.onClick.AddListener(() => HandleCancel());
        b.GetComponentInChildren<TMP_Text>().text = "Cancel";
        b.interactable = true;
    }

    public void SelectAttackType(SrpgAttack attack){
        selectedUnit.Attack(attack);
        Close();
    }

    public void DestroyButtons(){
        buttonContainer.transform.DeleteAllChildren();
    }

    protected override void HandleCancel(){
        if(!selectedUnit){
            Debug.LogWarning("No selectedUnit! (Maybe the menu should have stayed disabled.)");
            return;
        }
        switch (selectedUnit.state) {
            case SrpgUnit.State.SelectingAttackType:
                selectedUnit.ToSelectingAttackTarget(); Close(); break;
            default:
                audioSource.PlaySound(ESrpgSound.Buzzer); break;
        }
    }
}