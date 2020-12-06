using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Material materialSelected;
    public Material materialClicked;

    private Renderer myRenderer;
    private Material myMaterial;
    virtual protected void Awake()
    {
        myRenderer = this.GetComponent<Renderer>();
        myMaterial = myRenderer.material;

    }

    internal void Select()
    {
        if(myRenderer) myRenderer.material = materialSelected;
    }

    internal void Deselect()
    {
        if(myRenderer) myRenderer.material = myMaterial;
    }

    public void OnClick()
    {
        if(myRenderer) myRenderer.material = materialClicked;
        OnClickDown();
    }

    protected virtual void OnClickDown() { }
}
