
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
        items[itemType] = MathUtils.ClampToPositive(items[itemType]);
    }

    public bool HasAny(EItemType itemType) => items.ContainsKey(itemType) && items[itemType] > 0;
}