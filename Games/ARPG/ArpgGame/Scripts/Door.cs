using UnityEngine;
using System.Collections;

public class Door : PickupItem
{
    public EArpgGameItemType keyType = EArpgGameItemType.KEY;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"
            && FindObjectOfType<ArpgGamePlayerInventory>().HasAnyOfType(keyType) ){
            PickUp(keyType, -1);
        }
    }
}
