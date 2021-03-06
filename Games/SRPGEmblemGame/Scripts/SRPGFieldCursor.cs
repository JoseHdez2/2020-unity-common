﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpritePopInOut))]
public class SrpgFieldCursor : LerpMovement
{
    // GameObject references
    private SrpgAudioSource audioSource;
    private SrpgPrefabContainer prefabContainer;
    private SrpgUnitCard unitCard;
    private Collider2D cursorColl;
    private SrpgMenuUnit unitMenu;
    private SrpgMenuAttackType attackTypeMenu;
    private SpritePopInOut spritePopInOut;
    // "Pointers"
    public BoxCollider2D levelBoundsColl;
    public SRPGUnit selectedUnit;
    [SerializeField] private SRPGUnit hoveringUnit;
    [SerializeField] private SrpgTile hoveringTile;
    [Header("Settings")]
    [Range(0,0.5f)]
    public float deadzone = 0.05f;
    [Range(0,1)]
    [Tooltip("Seconds before the cursor will listen to an input again.")]
    public float cursorCooldown = 0.5f;
    private float curCursorCooldown = 0.01f;
    // Settings
    public bool showUnitCard = true;

    Vector2 lastMousePos; // TODO

    void Awake(){
        audioSource = FindObjectOfType<SrpgAudioSource>();
        unitCard = FindObjectOfType<SrpgUnitCard>();
        unitMenu = FindObjectOfType<SrpgMenuUnit>();
        attackTypeMenu = FindObjectOfType<SrpgMenuAttackType>();
        prefabContainer = FindObjectOfType<SrpgPrefabContainer>();
        cursorColl = GetComponent<Collider2D>();
        spritePopInOut = GetComponent<SpritePopInOut>();
        lastMousePos = Input.mousePosition;
    }

    private void OnEnable() {
        SetHoverFromCurPos();
        UpdateUnitCard(show: showUnitCard, hoveringUnit);
    }

    public void SelfDisable(){
        spritePopInOut.SelfDisable();
    }

    private void OnDisable() {
        UpdateUnitCard(show: false, null);
    }

    // Update is called once per frame
    new void Update(){
        // Vector2 mousePos = Input.mousePosition;
        // if(mousePos != lastMousePos){
        //     if(!cursorColl.bounds.Contains(mousePos)){
        //         destinationPos = mousePos;
        //     }
        //     lastMousePos = mousePos;
        // }
        if(destinationPos.HasValue){
            base.Update();
            return;
        }
        if(selectedUnit && selectedUnit.state == SRPGUnit.State.Moving){ // TODO remove this
            return;
        }
        if(curCursorCooldown > 0){
            curCursorCooldown -= Time.deltaTime;
            return;
        }
        if(Input.GetAxis("Horizontal") < -deadzone){
            MoveCursor(transform.position + Vector3.left);
        } else if (Input.GetAxis("Horizontal") > deadzone){
            MoveCursor(transform.position + Vector3.right);
        } else if (Input.GetAxis("Vertical") > deadzone){
            MoveCursor(transform.position + Vector3.up);
        } else if (Input.GetAxis("Vertical") < -deadzone){
            MoveCursor(transform.position + Vector3.down);
        } else if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit")) {
            HandleConfirm();
        } else if (Input.GetButtonDown("Cancel")){
            HandleCancel();
        }
    }

    private void HandleConfirm(){
        SetHoverFromCurPos();
        if (selectedUnit && hoveringTile){
            switch (hoveringTile.highlightType){
                case SrpgTile.Highlight.Move:
                    selectedUnit.Move(transform.position);
                    break;
                case SrpgTile.Highlight.Attack:
                    if(selectedUnit.state == SRPGUnit.State.SelectingAttackTarget && hoveringUnit && selectedUnit.CanAttack(hoveringUnit)){ // TODO recheck whether these checks are ok.
                        attackTypeMenu.Open(selectedUnit, hoveringUnit);
                        // selectedUnit.Attack(hoveringUnit);
                    } else {
                        audioSource.PlaySound(ESrpgSound.Buzzer);
                    }
                    break;
                default:
                    audioSource.PlaySound(ESrpgSound.Buzzer); break;
            }
        } else if (!selectedUnit && hoveringUnit && hoveringUnit.state != SRPGUnit.State.Spent) {
            SelectUnit(hoveringUnit);
        } else {
            audioSource.PlaySound(ESrpgSound.Buzzer);
        }
    }

    private void HandleCancel(){
        SetHoverFromCurPos();
        if(selectedUnit){
            switch (selectedUnit.state)
            {
                case SRPGUnit.State.SelectingMove:
                    selectedUnit.ToIdle();
                    selectedUnit = null;    
                    audioSource.PlaySound(ESrpgSound.Cancel);
                    break;
                case SRPGUnit.State.SelectingAttackTarget:
                    unitMenu.Open(selectedUnit);
                    audioSource.PlaySound(ESrpgSound.Cancel);
                    break;
                default:
                    audioSource.PlaySound(ESrpgSound.Buzzer); break;
            }
        } else {
            audioSource.PlaySound(ESrpgSound.Buzzer);
        }
    }

    public void OpenUnitMenu(){
        unitMenu.Open(selectedUnit);
    }

    public void SelectUnit(SRPGUnit unitToSelect){
        destinationPos = unitToSelect.transform.position;
        audioSource.PlaySound(ESrpgSound.SelectUnit);
        selectedUnit = unitToSelect;
        if(selectedUnit.state == SRPGUnit.State.Idle){
            selectedUnit.ToSelectingMove();
        }
    }

    private void MoveCursor(Vector3 pos){
        // if(!levelBoundsColl.bounds.Contains(pos)){
        //     audioSource.PlaySound(ESRPGSound.Buzzer);
        //     return;
        // }
        ClearHover();
        audioSource.PlaySound(ESrpgSound.FieldCursor);
        destinationPos = pos;
        curCursorCooldown = cursorCooldown;
    }

    public void MoveCursorAndConfirm(Vector3 pos){
        StartCoroutine(CrMoveCursorAndConfirm(pos));
    }

    private IEnumerator CrMoveCursorAndConfirm(Vector3 pos){
        MoveCursor(pos);
        yield return new WaitUntil(() => !destinationPos.HasValue);
        HandleConfirm();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        SetHover(other);
    }

    // TODO maybe delete this.
    private void SetHoverFromCurPos(){
        var newHoveringTile = FindObjectsOfType<SrpgTile>().FirstOrDefault(o => cursorColl.bounds.Contains(o.transform.position));
        var newHoveringUnit = FindObjectsOfType<SRPGUnit>().FirstOrDefault(o => cursorColl.bounds.Contains(o.transform.position));
        SetHover(newHoveringTile, newHoveringUnit);
    }

    private void SetHover(Collider2D other){
        var newHoveringTile = other?.gameObject.GetComponent<SrpgTile>();
        var newHoveringUnit = other?.gameObject.GetComponent<SRPGUnit>();
        SetHover(newHoveringTile, newHoveringUnit);
    }

    private void SetHover(SrpgTile newHoveringTile, SRPGUnit newHoveringUnit){
        if(newHoveringTile){
            hoveringTile = newHoveringTile;
        } else if (newHoveringUnit){
            hoveringUnit = newHoveringUnit;
            UpdateUnitCard(showUnitCard, hoveringUnit);
        }
    }

    private void ClearHover(){
        hoveringTile = null;
        hoveringUnit = null;
        UpdateUnitCard(false, hoveringUnit);
    }

    private void UpdateUnitCard(bool show, SRPGUnit hoveringUnit){
        if(show && hoveringUnit){
            unitCard.Open();
            
            unitCard.SetUnit(hoveringUnit);
        } else {
            unitCard.Close();
        }
    }
}
