using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    public Material materialSelected;
    public Material materialClicked;

    private Renderer myRenderer;
    private Material myMaterial;

    [SerializeField] private UnityEvent action;
    public bool selected;

    virtual protected void Awake()
    {
        myRenderer = this.GetComponent<Renderer>();
        if(myRenderer) {
            myMaterial = myRenderer.material;
        }

    }

    internal void Select()
    {
        if(myRenderer) { myRenderer.material = materialSelected; }
        selected = true;
    }

    internal void Deselect()
    {
        if(myRenderer) { myRenderer.material = myMaterial; }
        selected = false;
    }

    public void OnClick()
    {
        if(selected){
            if(myRenderer) { myRenderer.material = materialClicked; }
            if(action != null){
                action.Invoke();
            }
            OnClickDown();
        }
    }

    protected virtual void OnClickDown() { }
}
