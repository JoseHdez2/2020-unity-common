using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDropWorkbench : MonoBehaviour
{
    [SerializeField] DragDropSlot inputSlot;
    [SerializeField] DragDropSlot outputSlot;

    public float productionTime = 1;
    private bool producing = false;
    private float productionTimeLeft;
    public Image loadingBar;

    [SerializeField] GameObject itemDraggablePrefab;
    [SerializeField] Sprite outputSprite;
    [SerializeField] ItemStackable outputItem;

    public void StartProduction(){
        StartCoroutine(FinishProduction());
    }

    public void Start(){
        productionTimeLeft = productionTime;
    }

    public void Update(){
        if(!producing && GetQueuedProducts() <= 0) { 
            // NOTE we should technically do "productionTimeLeft = productionTime" here but it would be too expensive. better leave it be.
            return; 
        }
        if(!producing){
            DecrementInputSlot();
            producing = true;
        }
        
        productionTimeLeft -= Time.deltaTime;
        loadingBar.fillAmount = 1 - (productionTimeLeft / productionTime);

        if(productionTimeLeft <= 0){
            IncrementOutputSlot();
            producing = false;
            if(GetQueuedProducts() > 0){
                productionTimeLeft += productionTime;
            } else {
                CleanState();
            }
        }
    }

    public void CleanState(){
        // SetQueuedProducts(0);
        loadingBar.fillAmount = 0;
        productionTimeLeft = productionTime;
    }
    
    public IEnumerator FinishProduction(){
        yield return new WaitForSeconds(productionTime);
    }

    int GetQueuedProducts(){
        return inputSlot.itemInSlot ? inputSlot.itemInSlot.GetAmount() : 0;
    }

    void DecrementInputSlot(){
        if(inputSlot.itemInSlot){
            inputSlot.itemInSlot.Decrement();
        } else {
            Debug.LogError("Cannot decrement input, as there is nothing there.");
        }
    }

    void IncrementOutputSlot(){
        if(outputSlot.itemInSlot){
            outputSlot.itemInSlot.Increment();
        } else {
            GameObject output = Instantiate(itemDraggablePrefab, new Vector3(), Quaternion.identity, outputSlot.transform);
            output.GetComponent<DragDropItem>().image.sprite = outputSprite;
            output.GetComponent<DragDropItem>().image.SetNativeSize();
            outputSlot.SetObjectInSlot(output);
        }
    }
}
