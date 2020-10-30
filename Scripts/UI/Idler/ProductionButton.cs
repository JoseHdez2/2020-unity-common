using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionButton : MonoBehaviour
{
    public int queuedProducts;
    public TMP_Text textQueuedProducts;
    public Image loadingBar;
    // Start is called before the first frame update
    public float productionTime = 1;
    private float productionTimeLeft;
    void Start()
    {
        CleanState();
    }

    // Update is called once per frame
    void Update()
    {
        if(queuedProducts <= 0) { return; }
        loadingBar.fillAmount = 1 - (productionTimeLeft / productionTime);
        productionTimeLeft -= Time.deltaTime;
        if(productionTimeLeft <= 0){
            SetQueuedProducts(queuedProducts - 1);
            // TODO add the product somewhere. to an inventory.
            if(queuedProducts > 0){
                productionTimeLeft += productionTime;
            } else {
                CleanState();
            }
        }
    }

    public void CleanState(){
        SetQueuedProducts(0);
        loadingBar.fillAmount = 0;
        productionTimeLeft = productionTime;
    }

    public void OnClick()
    {
        SetQueuedProducts(queuedProducts + 1);
    }

    void SetQueuedProducts(int i){
        queuedProducts = i;
        textQueuedProducts.text = i > 0 ? $"{i}" : "";
    }
}
