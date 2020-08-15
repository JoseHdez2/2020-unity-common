using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum DungeonCrawlerTile
{
    NONE, WALL, ENEMY, DOOR, KEY, PLAYER
}

[System.Serializable]
public class DungeonCrawlerRow : List<DungeonCrawlerTile> { };

[CreateAssetMenu(fileName = "DungeonCrawlerDb", menuName = "ScriptableObjects/DungeonCrawlerDatabase", order = 1)]
public class DungeonCrawlerDatabase : ScriptableObject
{
    public SerializableDictionaryBase<char, DungeonCrawlerTile> charToTile;
    public SerializableDictionaryBase<DungeonCrawlerTile, GameObject> tileToPrefab;
}
