using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;

    public Transform crosshair;
    private Transform curSelectionTransform;
    private Selectable curSelection;
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(crosshair.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            var transform = hit.transform;
            if (transform != curSelectionTransform){
                if (curSelection) { curSelection.Deselect(); }
                curSelectionTransform = null;
                Selectable selection = transform.GetComponent<Selectable>();
                if(selection){
                    selection.Select();
                    curSelection = selection;
                    curSelectionTransform = transform;
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
}
