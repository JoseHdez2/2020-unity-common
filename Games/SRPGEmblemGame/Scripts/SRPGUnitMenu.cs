﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRPGUnitMenu : ButtonMenuBase
{
    [SerializeField] private ImageWipe buttonContainer;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button moveButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button statusButton;
    [SerializeField] private Button waitButton;
    [SerializeField] private Button cancelButton;

    private SrpgAudioSource audioSource;
    private SrpgController srpgController;
    private SRPGUnit selectedUnit;

    public bool showStatusButton;
    public bool showCancelButton;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<SrpgAudioSource>();
        srpgController = FindObjectOfType<SrpgController>();
        buttonContainer.ToggleWipe(false);
    }

    public void OnEnable(){
        base.OnEnable();

        selectedButton = null;
    }

    public void Open(SRPGUnit unit){
        buttonContainer.ToggleWipe(true);
        srpgController.ToggleFieldCursor(false);
        gameObject.SetActive(true);
        HideIrrelevantButtons(unit);
        audioSource.PlaySound(ESRPGSound.SelectUnit);
        ResetCursor();
        selectedUnit = unit;
    }

    // All buttons reactivate by default. Deactivate the relevant ones.
    private void HideIrrelevantButtons(SRPGUnit unit){
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
        if(!showStatusButton){
            statusButton.gameObject.SetActive(false);
        }
        if(!showCancelButton){
            cancelButton.gameObject.SetActive(false);
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
        Close();
    }

    public void HandleCancel(){
        Debug.Log("HandleCancel");
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
        } else {
            audioSource.PlaySound(ESRPGSound.Buzzer);
        }
    }

    public void Close(){
        buttonContainer.ToggleWipe(false);
        srpgController.ToggleFieldCursor(true);
        audioSource.PlaySound(ESRPGSound.Cancel);
        gameObject.SetActive(false);
    }    

    // Must reset to null each time the menu becomes inactive. Only used for making a sound when the selected button changes.
    private GameObject selectedButton;

    // Update is called once per frame
    void Update()
    {         
        if (eventSystem.currentSelectedGameObject != selectedButton) {
            if(selectedButton != null){ // Avoid playing when the first button becomes selected automatically. but play thereafter.
                audioSource.PlaySound(ESRPGSound.MenuCursor);
            }
            selectedButton = eventSystem.currentSelectedGameObject;
        }
        if(Input.GetButtonDown("Cancel")){
            HandleCancel();
        }
    }
}
