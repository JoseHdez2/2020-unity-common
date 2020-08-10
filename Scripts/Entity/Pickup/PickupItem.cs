using UnityEngine;
using System.Collections;
using System.Linq;
using System;

[RequireComponent(typeof(AudioSource))]
public class ItemPickup<EItemType> : Expirable
    where EItemType : struct, IConvertible, IComparable, IFormattable
{
    public EItemType itemType;
    public int qty = 1;

    public void PickUpByPlayer(EItemType itemType, int qty)
    {
        FindObjectOfType<Inventory<EItemType>>().AddItem(itemType, qty);
        Expire();
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PickUpByPlayer(itemType, qty);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickUpByPlayer(itemType, qty);
        }
    }

}