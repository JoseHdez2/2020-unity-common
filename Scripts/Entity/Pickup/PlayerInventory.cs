
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

[Serializable]
public abstract class Inventory<EArpgItemType> : MonoBehaviour
    where EArpgItemType : struct, IConvertible, IComparable, IFormattable
{
    [SerializeField]
    SerializableDictionaryBase<EArpgItemType, int> items;

    public void AddItem(EArpgItemType itemType, int qty)
    {
        if (!items.ContainsKey(itemType))
        {
            items.Add(itemType, qty);
        }
        else
        {
            items[itemType] += qty;
        }
        items[itemType] = items[itemType].ClampToPositive();
    }

    public bool HasAny(EArpgItemType itemType) => items.ContainsKey(itemType) && items[itemType] > 0;
}