using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SrpgMenuUnit : SrpgMenuBase
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button moveButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button statusButton;
    [SerializeField] private Button waitButton;
    [SerializeField] private Button cancelButton;

    public void Open(SRPGUnit unit){
        buttonContainer.Toggle(true);
        srpgController.ToggleFieldCursorFalse();
        gameObject.SetActive(true);
        HideIrrelevantButtons(unit);
        audioSource.PlaySound(ESrpgSound.SelectUnit);
        ResetCursor();
        selectedUnit = unit;
    }

    // All buttons reactivate by default. Deactivate the relevant ones.
    private void HideIrrelevantButtons(SRPGUnit unit){
        if(!unit.CanAttackSomeTarget()){
            attackButton.gameObject.SetActive(false);
        }
        if(!unit.HasItem()){
            itemButton.gameObject.SetActive(false);
        }
        if(unit.state != SRPGUnit.State.Idle){
            moveButton.gameObject.SetActive(false);
        }
        if(unit.state == SRPGUnit.State.Spent){
            waitButton.gameObject.SetActive(false);
        }
        if(!srpgController.settings.showStatusButton){
            statusButton.gameObject.SetActive(false);
        }
        if(!srpgController.settings.showCancelButton){
            cancelButton.gameObject.SetActive(false);
        }
    }

    public void HandleAttack(){
        selectedUnit.ToSelectingAttackTarget();
        Close();
    }

    public void HandleMove(){
        audioSource.PlaySound(ESrpgSound.Buzzer);
    }

    public void HandleItem(){
        audioSource.PlaySound(ESrpgSound.Buzzer);
    }

    public void HandleStatus(){
        audioSource.PlaySound(ESrpgSound.Buzzer);
    }

    public void HandleWait(){
        selectedUnit.ToSpent(checkForTurnChange: false); // We're already going to check when this menu is closed!
        Close();
    }

    protected override void HandleCancel(){
        if(!selectedUnit){
            Debug.LogWarning("UnitMenu has no selectedUnit! (Maybe the menu should have been disabled.)");
            return;
        }
        if(selectedUnit.state == SRPGUnit.State.Moved){
            selectedUnit.ToIdle();
            Close();
        } else if(selectedUnit.state == SRPGUnit.State.SelectingMove) {
            selectedUnit.ToIdle();
            Close();
        } else if(selectedUnit.state == SRPGUnit.State.SelectingAttackTarget) {
            selectedUnit.ToSelectingMove();
            Close();
        } else {
            audioSource.PlaySound(ESrpgSound.Buzzer);
        }
    }

}
