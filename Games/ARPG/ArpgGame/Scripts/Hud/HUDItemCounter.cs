using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDItemCounter : MonoBehaviour
{
    public List<ItemAndQty> ItemRefs = new List<ItemAndQty>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateInventoryTexts(EArpgGameItemType itemTypeArg, int qty)
    {
        Debug.Log("updateInventoryTexts called");
        foreach (ItemAndQty itemref in ItemRefs)
        {
            if(itemref.itemType == itemTypeArg)
            {
                itemref.reference.text = qty.ToString();
            }
        }
    }
}

 [System.Serializable]
public class ItemAndQty
{
    public EArpgGameItemType itemType;
    public TMP_Text reference;
}