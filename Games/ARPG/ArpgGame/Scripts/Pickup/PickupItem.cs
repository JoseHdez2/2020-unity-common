using UnityEngine;
using System.Collections;
using System.Linq;

public enum EPickupType
{
    HEALTH,
    ITEM, // Inventory item: arrows, bombs...
    MAX_HEALTH,
    MAX_MONEY,
    MONEY
}

public class PickupItem : Expirable
{
    public EPickupType pickupType;
    public EArpgGameItemType itemType;
    public int qty = 1;
    
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PickUp(other.gameObject, pickupType, itemType, qty);
        }
    }

    public void PickUp(GameObject obj, EPickupType pickupType, EArpgGameItemType itemType, int qty)
    {
        switch (pickupType)
        {
            case EPickupType.HEALTH: Heal(obj, qty); break;
            case EPickupType.ITEM: PickUp(itemType, qty); break;
            case EPickupType.MAX_HEALTH: ExpandMaxHealth(obj, qty); break;
        }
    }

    public void Heal(GameObject obj, int qty)
    {
        EntityDamageable ed = obj.GetComponent<EntityDamageable>();
        ed.Heal(qty);
        Expire();
    }

    public void ExpandMaxHealth(GameObject obj, int qty)
    {
        EntityDamageable ed = obj.GetComponent<EntityDamageable>();
        ed.ExpandMaxHealth(qty);
        Expire();
    }

    public void PickUp(EArpgGameItemType itemType, int qty)
    {
        FindObjectOfType<ArpgGamePlayerInventory>().AddItem(itemType, qty);
        Expire();
    }
}
