using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SrpgFieldCursor : LerpMovement
{
    // GameObject references
    private SrpgAudioSource audioSource;
    private SRPGUnitCard unitCard;
    private SRPGUnitMenu unitMenu;
    [SerializeField] private GameObject pfTileMove; 
    [SerializeField] private GameObject pfTileAttack; 
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

    void Awake()
    {
        audioSource = FindObjectOfType<SrpgAudioSource>();
        unitCard = FindObjectOfType<SRPGUnitCard>();
        unitMenu = FindObjectOfType<SRPGUnitMenu>();
    }

    private void OnEnable() {
        UpdateUnitCard(show: showUnitCard, hoveringUnit);
    }

    private void OnDisable() {
        UpdateUnitCard(show: false, null);
    }


    // Update is called once per frame
    void Update()
    {
        if(destinationPos.HasValue){
            base.Update();
            return;
        }
        if(selectedUnit && selectedUnit.state == SRPGUnit.State.Moving){
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
        }
        else if (Input.GetButtonDown("Cancel")){
            if(selectedUnit){
                if(selectedUnit.state == SRPGUnit.State.SelectingMove){
                    selectedUnit.ToIdle();
                    selectedUnit = null;
                    audioSource.PlaySound(ESRPGSound.Cancel);
                } else {
                    audioSource.PlaySound(ESRPGSound.Buzzer);
                }
            } else {
                audioSource.PlaySound(ESRPGSound.Buzzer);
            }
        }
    }

    private void HandleConfirm()
    {
        if (selectedUnit && selectedUnit.state == SRPGUnit.State.SelectingMove && hoveringTile && hoveringTile.highlightType == SrpgTile.Highlight.Move){
            selectedUnit.Move(transform.position);
        } else if (!selectedUnit && hoveringUnit && hoveringUnit.state != SRPGUnit.State.Spent) {
            SelectUnit();
        } else {
            audioSource.PlaySound(ESRPGSound.Buzzer);
        }
    }

    public void OpenUnitMenu(){
        unitMenu.Open(selectedUnit);
    }

    private void SelectUnit(){
        audioSource.PlaySound(ESRPGSound.SelectUnit);
        selectedUnit = hoveringUnit;
        if(selectedUnit.state == SRPGUnit.State.Idle){
            selectedUnit.SpawnMoveTiles(pfTileMove, pfTileAttack);
        }
    }

    private void MoveCursor(Vector3 pos){
        // if(!levelBoundsColl.bounds.Contains(pos)){
        //     audioSource.PlaySound(ESRPGSound.Buzzer);
        //     return;
        // }
        ClearHover();
        audioSource.PlaySound(ESRPGSound.FieldCursor);
        destinationPos = pos;
        curCursorCooldown = cursorCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        SetHover(other);
    }

    private void SetHover(Collider2D other){
        var newHoveringTile = other?.gameObject.GetComponent<SrpgTile>();
        var newHoveringUnit = other?.gameObject.GetComponent<SRPGUnit>();
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
        Debug.Log($"UpdateUnitCard: {show},{hoveringUnit}");
        if(show && hoveringUnit){
            unitCard.Open();
            unitCard.SetUnit(hoveringUnit);
        } else {
            unitCard.Close();
        }
    }
}
