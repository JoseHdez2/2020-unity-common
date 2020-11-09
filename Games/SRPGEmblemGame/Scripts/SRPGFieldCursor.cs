using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRPGFieldCursor : LerpMovement
{
    // GameObject references
    private SRPGAudioSource audioSource;
    private SRPGUnitCard unitCard;
    private SRPGUnitMenu unitMenu;
    [SerializeField] private GameObject pfTile; 
    // "Pointers"
    public SRPGUnit selectedUnit;
    [SerializeField] private SRPGUnit hoveringUnit;
    [SerializeField] private SRPGTile hoveringTile;
    [Header("Settings")]
    [Range(0,0.5f)]
    public float deadzone = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<SRPGAudioSource>();
        unitCard = FindObjectOfType<SRPGUnitCard>();
        unitMenu = FindObjectOfType<SRPGUnitMenu>();
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
                } else {
                    
                }
            } else {
                audioSource.PlaySound(ESRPGSound.Buzzer);
            }
        }
    }

    private void HandleConfirm()
    {
        if (selectedUnit && selectedUnit.state == SRPGUnit.State.SelectingMove && hoveringTile){
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
            selectedUnit.SpawnMoveTiles(pfTile);
        }
    }

    private void MoveCursor(Vector3 pos){
        hoveringUnit = null;
        unitCard.Close();
        audioSource.PlaySound(ESRPGSound.FieldCursor);
        destinationPos = pos;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        hoveringUnit = other.gameObject.GetComponent<SRPGUnit>();
        hoveringTile = other.gameObject.GetComponent<SRPGTile>();
        if(hoveringUnit){
            unitCard.Open();
            unitCard.SetUnit(hoveringUnit);
        }
    }
}
