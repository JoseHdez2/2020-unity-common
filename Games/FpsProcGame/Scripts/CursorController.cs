using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    public Texture2D cursor;
    public Texture2D cursorClick;
    private CursorControls controls;

    private void Awake() {
        controls = new CursorControls();
        ChangeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        controls.Mouse.Click.started += _ => StartedClick();
        controls.Mouse.Click.performed += _ => EndedClick();
    }

    private void StartedClick(){
        ChangeCursor(cursorClick);
    }

    private void EndedClick(){
        ChangeCursor(cursor);
    }

    private void ChangeCursor(Texture2D cursorType){
        //Vector2 hotspot = new Vector2(0, cursorType.height);
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);        
    }
}
