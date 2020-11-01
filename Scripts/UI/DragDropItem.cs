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

    public void Add(int amount){
        item.amount += amount;
        textAmount.text = $"x{item.amount}";
    }

    public void Substract(int amount){
        item.amount -= amount;
        textAmount.text = $"x{item.amount}";
        if(item.amount <= 0){
            Destroy(this.gameObject);
        }
    }
}