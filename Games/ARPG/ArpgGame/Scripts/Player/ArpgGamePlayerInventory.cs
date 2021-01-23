using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using System.Linq;


public enum EArpgGameItemType
{
    ARROW,
    BOMB,
    BOW,
    KEY,
    POTION
}

public class Inventory
{
    public Dictionary<EArpgGameItemType, int> items = new Dictionary<EArpgGameItemType, int>();
    int money = 0;

    public void AddItem(EArpgGameItemType itemType, int qty)
    {
        if (!items.ContainsKey(itemType))
        {
            items.Add(itemType, qty);
        } else {
            items[itemType] += qty;
        }
    }
}

[Serializable]
public class ItemQty
{
    [SerializeField] public EArpgGameItemType Key;
    [SerializeField] public int Value;
}

public class ArpgGamePlayerInventory : MonoBehaviour
{
    [SerializeField] List<ItemQty> initialPlayerInventory = new List<ItemQty>();
    static Inventory playerInventory;
    public HUDItemCounter hudItemCounter;

    public void Awake()
    {
        if (playerInventory == null)
        {

            playerInventory = new Inventory();
            initialPlayerInventory.ForEach(p => playerInventory.items.Add(p.Key, p.Value));
        }
        new HUDItemCounter();
    }

    public void Start()
    {
        UpdateInventoryText();
    }

    public TMP_Text textInventory;

    public void AddItem(EArpgGameItemType itemType, int qty)
    {
        Debug.Log("Add item called");
        playerInventory.AddItem(itemType, qty);
        //UpdateInventoryText();
        hudItemCounter.updateInventoryTexts(itemType, playerInventory.items[itemType]);
    }

    // TODO RemoveItem method that checks that no -1 cases happen.

    public bool HasAnyOfType(EArpgGameItemType itemType)
    {
        return playerInventory.items.ContainsKey(itemType)
            && playerInventory.items[itemType] > 0;
    }

    private void UpdateInventoryText()
    {
        textInventory.text = "";
        foreach(KeyValuePair<EArpgGameItemType, int> entry in playerInventory.items)
        {
            textInventory.text += $"{entry.Key}: {entry.Value}\n";
        }
    }

    public void SaveInventory() { }

    public void LoadInventory() { } // PlayerPrefs
}
