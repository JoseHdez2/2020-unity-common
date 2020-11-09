using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRPGUnitMenu : ButtonMenuShowHide
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button moveButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button statusButton;
    [SerializeField] private Button waitButton;
    [SerializeField] private Button cancelButton;

    private SRPGAudioSource audioSource;
    private SRPGFieldCursor fieldCursor;
    private SRPGUnit selectedUnit;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        audioSource = FindObjectOfType<SRPGAudioSource>();
        fieldCursor = FindObjectOfType<SRPGFieldCursor>();
    }

    public void Show(SRPGUnit unit){
        base.Show();
        fieldCursor.gameObject.SetActive(false);
        audioSource.PlaySound(ESRPGSound.SelectUnit);
        HideIrrelevantButtons(unit);
        selectedUnit = unit;
    }

    public void Hide(){
        base.Hide();
        fieldCursor.gameObject.SetActive(true);
        audioSource.PlaySound(ESRPGSound.Cancel);
        gameObject.SetActive(false);
    }

    public void OnChangeSelection(){
        audioSource.PlaySound(ESRPGSound.MenuCursor);
    }

    // All buttons reactivate by default. Deactivate the relevant ones.
    private void HideIrrelevantButtons(SRPGUnit unit){
        Debug.Log("HideIrrelevantButtons");
        if(!unit.InAttackRange()){
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
    }

    public void HandleAttack(){
        audioSource.PlaySound(ESRPGSound.Buzzer);
    }

    public void HandleMove(){
        audioSource.PlaySound(ESRPGSound.Buzzer);
    }

    public void HandleItem(){
        audioSource.PlaySound(ESRPGSound.Buzzer);
    }

    public void HandleStatus(){
        audioSource.PlaySound(ESRPGSound.Buzzer);
    }

    public void HandleWait(){
        selectedUnit.ToSpent();
        Hide();
    }

    public void HandleCancel(){
        if(selectedUnit.state == SRPGUnit.State.Moved){
            selectedUnit.ToIdle();
            Hide();
        } else if(selectedUnit.state == SRPGUnit.State.SelectingMove) {
            selectedUnit.ToIdle();
            Hide();
        } else {
            audioSource.PlaySound(ESRPGSound.Buzzer);
        }
    }

}
