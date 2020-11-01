using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStackable{
    int id;
    string name;
    
    private List<ItemStackable> itemList;

    public InventoryStackable(){
        itemList = new List<ItemStackable>();

        AddItem(new ItemStackable {typeId = 1, amount = 1} );
    }

    public void AddItem(ItemStackable item){
        itemList.Add(item);
    }
}
