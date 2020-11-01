using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class DragDropWorkbench : MonoBehaviour
{
    [Header("GameObject Refs")]
    [SerializeField] DragDropSlot[] inputSlots;
    [SerializeField] DragDropSlot[] outputSlots;

    [SerializeField] Image loadingBar;

    [Header("Prefab Refs")]
    [SerializeField] GameObject itemDraggablePrefab;

    [Header("Crafting Recipe Details")]
    public float productionTime = 1;
    [SerializeField] Sprite outputSprite;
    [SerializeField] ItemStackable[] inputItems;
    [SerializeField] ItemStackable[] outputItems;
    
    private bool workshopIsProducing = false;
    private bool itemInProduction = false;
    private float productionTimeLeft;

    public void Start(){
        StopProduction();
    }

    public void StartProduction(){
        workshopIsProducing = true;
    }

    public void Update(){
        if(!workshopIsProducing) { return; } // Not producing.
        
        if(!itemInProduction){
            if(GetQueuedProducts() > 0){
                productionTimeLeft += productionTime;
                itemInProduction = true;
                DecrementInputSlots();
            } else {
                StopProduction();
            }
        } else {
            productionTimeLeft -= Time.deltaTime;
            loadingBar.fillAmount = 1 - (productionTimeLeft / productionTime);

            if(productionTimeLeft <= 0){
                IncrementOutputSlots();
                itemInProduction = false;
            }
        }
    }

    public void StopProduction(){
        loadingBar.fillAmount = 0;
        productionTimeLeft = 0;
        workshopIsProducing = false;
    }

    int GetQueuedProducts(){
        return inputItems.Select(input => GetQueuedProduct(input)).Max();
    }

    int GetQueuedProduct(ItemStackable input){
        DragDropSlot inputSlot = inputSlots.FirstOrDefault(slot => slot.HasItemOfType(input.typeId));
        return inputSlot == null ? 0 : inputSlot.itemInSlot.item.amount / input.amount;
    }

    void DecrementInputSlots(){
        foreach(ItemStackable inputItem in inputItems){
            DragDropSlot inputSlot = inputSlots.FirstOrDefault(slot => slot.HasItemOfType(inputItem.typeId));
            if(inputSlot && inputSlot.itemInSlot){
                inputSlot.itemInSlot.Substract(inputItem.amount);
            } else {
                Debug.LogError("Cannot decrement input, as there is nothing there.");
            }
        }
    }

    void IncrementOutputSlots(){
        foreach(ItemStackable outputItem in outputItems){
            DragDropSlot outputSlot = outputSlots.FirstOrDefault(slot => slot.HasItemOfType(outputItem.typeId));
            if(outputSlot && outputSlot.itemInSlot){
                outputSlot.itemInSlot.Add(outputItem.amount);
            } else {
                DragDropSlot firstEmptySlot = outputSlots.FirstOrDefault(slot => slot.itemInSlot == null);
                GameObject output = Instantiate(itemDraggablePrefab, new Vector3(), Quaternion.identity, firstEmptySlot.transform);
                DragDropItem newOutputItem = output.GetComponent<DragDropItem>();
                newOutputItem.image.sprite = outputSprite;
                newOutputItem.image.SetNativeSize();
                newOutputItem.item.typeId = outputItem.typeId;
                newOutputItem.item.amount = outputItem.amount;
                
                firstEmptySlot.SetObjectInSlot(output);
            }
        }
    }
}
