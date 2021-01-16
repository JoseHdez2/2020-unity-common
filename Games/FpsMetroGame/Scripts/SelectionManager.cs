using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;

    public Transform crosshair;
    private Transform curSelectionTransform;
    private Selectable curSelection;
    public float maxDistance = 10f;
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(crosshair.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            var candidateTransform = hit.transform;
            if (candidateTransform != curSelectionTransform){
                if (curSelection) { curSelection.Deselect(); }
                // if (Vector3.Distance(transform.position, candidateTransform.position) < maxDistance){ // TODO
                    curSelectionTransform = null;
                    Selectable selection = candidateTransform.GetComponent<Selectable>();
                    if(selection){
                        selection.Select();
                        curSelection = selection;
                        curSelectionTransform = candidateTransform;
                    }
                // }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (curSelection){
                curSelection.OnClick();
            }
        }
    }
}
