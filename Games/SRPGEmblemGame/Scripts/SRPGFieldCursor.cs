using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRPGFieldCursor : MonoBehaviour
{
    // GameObject references
    private SRPGAudioSource audioSource;
    private SRPGUnitCard unitCard;
    private SRPGUnitMenu unitMenu;
    // "Pointers"
    private Vector3? destinationPos;
    private SRPGUnit selectedUnit;
    [Header("Settings")]
    [Range(0,1)]
    public float cursorSpeed = 0.05f;
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
            transform.position = Vector3.Lerp(transform.position, destinationPos.Value, cursorSpeed);
            if(Vector3.Distance(transform.position, destinationPos.Value) < 0.05){
                transform.position = destinationPos.Value;
                destinationPos = null;
            }
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
        } else if (Input.GetButtonDown("Jump") && selectedUnit){
            unitMenu.Open();
        }
    }

    private void MoveCursor(Vector3 pos){
        selectedUnit = null;
        audioSource.PlaySound(ESRPGSound.Move);
        destinationPos = pos;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        selectedUnit = other.gameObject.GetComponent<SRPGUnit>();
        if(selectedUnit){
            unitCard.Open();
            unitCard.SetUnit(selectedUnit);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        selectedUnit = null;
        unitCard.Close();
    }
}
