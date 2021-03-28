using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;

    public Transform crosshair;
    private Transform curSelectionTransform;
    private Selectable curSelection;    
    public float maxDistance = 10f;
    private Image crosshairRenderer;
    public Color colorNeutral, colorSelected, colorFar;
    public bool debugTexts = false;
    public TMP_Text textSelectionTransform, textSelection;

    private void Start() {
        crosshairRenderer = crosshair.GetComponent<Image>();
        Debug.Log(crosshairRenderer);
    }
    void Update()
    {
        if(debugTexts && textSelectionTransform) { textSelectionTransform.text = curSelectionTransform?.name ?? "..."; }
        if(debugTexts && textSelection) { textSelection.text = curSelection?.name ?? "..."; }

        var ray = Camera.main.ScreenPointToRay(crosshair.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            var candidateTransform = hit.transform;
            bool tooFar = Vector3.Distance(transform.position, candidateTransform.position) > maxDistance;
            if (tooFar) { Deselect(); crosshairRenderer.color = colorFar; }
            if (candidateTransform != curSelectionTransform){                
                if (curSelection) { Deselect(); crosshairRenderer.color = colorNeutral; }                
                curSelectionTransform = null;
                Selectable selection = candidateTransform.GetComponent<Selectable>();
                if(selection && !tooFar){
                    selection.Select();
                    crosshairRenderer.color = colorSelected;
                    curSelection = selection;
                    curSelectionTransform = candidateTransform;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (curSelection){
                curSelection.OnClick();
            }
        }
    }

    private void Deselect(){
        curSelection.Deselect();
        curSelectionTransform = null;
    }
}
