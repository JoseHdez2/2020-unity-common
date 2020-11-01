using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragDropSlot : MonoBehaviour, IDropHandler
{
    public DragDropItem itemInSlot;
    private GameObject objectInSlot;
    public bool canDragInto = true;
    public bool canDragOutOf = true;
    public void OnDrop(PointerEventData eventData){
        if(eventData.pointerDrag == null){ return; }
        if(itemInSlot == null) {
            SetObjectInSlot(eventData.pointerDrag.gameObject);
        } else {
            DragDropItem newItem = eventData.pointerDrag.gameObject.GetComponent<DragDropItem>();
            if(itemInSlot.item.typeId == newItem.item.typeId){
                itemInSlot.Add(newItem.item.amount);
                Destroy(newItem.gameObject);
            } else {
                Debug.LogError("Tried to put an incompatible item into an occupied slot.");
            }
        }
        SetObjectInSlot(eventData.pointerDrag.gameObject);
    }

    public void SetObjectInSlot(GameObject gameObject){
        objectInSlot = gameObject;
        objectInSlot.transform.SetParent(this.gameObject.transform, worldPositionStays: true); 
        objectInSlot.transform.localPosition = new Vector3();
        Debug.Log(objectInSlot);
        itemInSlot = objectInSlot.GetComponent<DragDropItem>();
    }

    public bool HasItemOfType(int typeId){
        return itemInSlot != null && itemInSlot.item.typeId == typeId;
    }
}
