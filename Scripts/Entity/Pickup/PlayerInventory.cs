
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

[Serializable]
public abstract class Inventory<EItemType> : MonoBehaviour
    where EItemType : struct, IConvertible, IComparable, IFormattable
{
    [SerializeField]
    SerializableDictionaryBase<EItemType, int> items;

    public void AddItem(EItemType itemType, int qty)
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

    public bool HasAny(EItemType itemType) => items.ContainsKey(itemType) && items[itemType] > 0;
}