using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public void AddItem(TKey itemType, TValue qty)
    {
        if (!ContainsKey(itemType))
        {
            Add(itemType, qty);
        } else {
            this[itemType] = qty;
        }
    }
}
