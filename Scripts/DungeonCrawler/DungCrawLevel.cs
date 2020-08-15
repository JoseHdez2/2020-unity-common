using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "level_", menuName = "ScriptableObjects/DungCrawLevel")]
public class DungCrawLevel : ScriptableObject
{
    [TextArea(10,10)]
    public string level;
    public int playerMaxHealth;
}
