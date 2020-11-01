using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropSlot : MonoBehaviour, IDropHandler
{
    public DragDropItem itemInSlot;
    private GameObject objectInSlot;
    public bool canDragInto = true;
    public bool canDragOutOf = true;
    public void OnDrop(PointerEventData eventData){
        if(eventData.pointerDrag != null){
            SetObjectInSlot(eventData.pointerDrag.gameObject);
        }
    }

    public void SetObjectInSlot(GameObject gameObject){
        objectInSlot = gameObject;
        objectInSlot.transform.SetParent(this.gameObject.transform, worldPositionStays: true); 
        objectInSlot.transform.localPosition = new Vector3();
        Debug.Log(objectInSlot);
        itemInSlot = objectInSlot.GetComponent<DragDropItem>();
    }
}
