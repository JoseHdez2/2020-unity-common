using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragDropSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
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
                itemInSlot.Add(newItem.item.amount); // Combine.
                Destroy(newItem.gameObject);
            } else {
                Debug.LogError("Tried to put an incompatible item into an occupied slot.");
            }
        }
    }

    public void SetObjectInSlot(GameObject gameObject){
        objectInSlot = gameObject;
        objectInSlot.transform.SetParent(this.gameObject.transform, worldPositionStays: true); 
        objectInSlot.transform.localPosition = new Vector3();
        itemInSlot = objectInSlot.GetComponent<DragDropItem>();
        itemInSlot.EndDrag();

        DragDropWorkbench workbench = GetComponentInParent<DragDropWorkbench>();
        if(workbench != null){
            workbench.StartProduction();
        }
    }

    public bool HasItemOfType(int typeId){
        return itemInSlot != null && itemInSlot.item.typeId == typeId;
    }
    public void OnPointerDown(PointerEventData eventData){
        Debug.Log(eventData.button);
        if(eventData.button == PointerEventData.InputButton.Right){
            DragDropItem myItem = itemInSlot;
            myItem.DetachFromSlot();
            FindObjectOfType<InventoryPanel>().AddItem(myItem);
            return;
        }
    }
}
