using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListOverflowException : Exception {
    public ListOverflowException(string msg) : base(msg) { }
}

/// <summary>
/// List with a max capacity that may not be exceeded.
/// </summary>
/// <typeparam name="T"></typeparam>
public class FiniteList<T> : List<T> {
    public FiniteList(int capacity) : base(capacity) {}

    public new void Add(T item){
        if(Count == Capacity) {
            throw new ListOverflowException($"Item would exceed max list capacity of {Count}!");
        } else {
            base.Add(item);
        }
    }
}

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] public static FiniteList<ItemType> playerInventory = new FiniteList<ItemType>(10);
    public static int money;

    public static void DiscardItem(int index){
        playerInventory.RemoveAt(index);
    }

    public static string GetItemName(int index) => playerInventory[index].ToString().Replace("_", " ");
}
