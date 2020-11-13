using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SrpgFieldCursor : LerpMovement
{
    // GameObject references
    private SrpgAudioSource audioSource;
    private SrpgUnitCard unitCard;
    private SrpgUnitMenu unitMenu;
    [SerializeField] private GameObject pfTileMove; 
    [SerializeField] private GameObject pfTileAttack; 
    // "Pointers"
    public BoxCollider2D levelBoundsColl;
    public SrpgUnit selectedUnit;
    [SerializeField] private SrpgUnit hoveringUnit;
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
        unitCard = FindObjectOfType<SrpgUnitCard>();
        unitMenu = FindObjectOfType<SrpgUnitMenu>();
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
        if(selectedUnit && selectedUnit.state == SrpgUnit.State.Moving){
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
            HandleCancel();
        }
    }

    private void HandleConfirm(){
        if (selectedUnit && hoveringTile){
            switch (hoveringTile.highlightType){
                case SrpgTile.Highlight.Move:
                    selectedUnit.Move(transform.position);
                    break;
                case SrpgTile.Highlight.Attack:
                    if(selectedUnit.state == SrpgUnit.State.SelectingAttack && hoveringUnit){ // TODO recheck whether these checks are ok.
                        selectedUnit.Attack(hoveringUnit);
                    } else {
                        audioSource.PlaySound(ESRPGSound.Buzzer);
                    }
                    break;
                default:
                    audioSource.PlaySound(ESRPGSound.Buzzer); break;
            }
        } else if (!selectedUnit && hoveringUnit && hoveringUnit.state != SrpgUnit.State.Spent) {
            SelectUnit();
        } else {
            audioSource.PlaySound(ESRPGSound.Buzzer);
        }
    }

    private void HandleCancel(){
        if(selectedUnit){
            switch (selectedUnit.state)
            {
                case SrpgUnit.State.SelectingMove:
                    selectedUnit.ToIdle();
                    selectedUnit = null;    
                    audioSource.PlaySound(ESRPGSound.Cancel);
                    break;
                case SrpgUnit.State.SelectingAttack:
                    unitMenu.Open(selectedUnit);
                    audioSource.PlaySound(ESRPGSound.Cancel);
                    break;
                default:
                    audioSource.PlaySound(ESRPGSound.Buzzer); break;
            }
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
        if(selectedUnit.state == SrpgUnit.State.Idle){
            selectedUnit.ToSelectingMove(pfTileMove, pfTileAttack);
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
        var newHoveringUnit = other?.gameObject.GetComponent<SrpgUnit>();
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

    private void UpdateUnitCard(bool show, SrpgUnit hoveringUnit){
        if(show && hoveringUnit){
            unitCard.Open();
            unitCard.SetUnit(hoveringUnit);
        } else {
            unitCard.Close();
        }
    }
}
