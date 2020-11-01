using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DragDropItem : DragDrop {

    public ItemStackable item = new ItemStackable();

    public Image image;
    [SerializeField] private TMP_Text textAmount;

    public void Start(){
        textAmount.text = $"x{item.amount}";
    }

    public int GetAmount(){ return item.amount; }

    public void Increment(){
        item.amount++;
        textAmount.text = $"x{item.amount}";
    }

    public void Decrement(){
        item.amount--;
        textAmount.text = $"x{item.amount}";
        if(item.amount <= 0){
            Destroy(this.gameObject);
        }
    }
}