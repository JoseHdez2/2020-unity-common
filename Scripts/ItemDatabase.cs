using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    NONE, COPPER_ORE, IRON_ORE, TITANIUM_ORE, OAK_WOOD, BIRCH_WOOD, ASH_WOOD, COPPER_SHOVEL,
    COPPER_PICKAXE, COPPER_AXE, IRON_SHOVEL, IRON_PICKAXE, IRON_AXE, TITANIUM_SHOVEL,
    TITANIUM_PICKAXE, TITANIUM_AXE, HERB, CELERY, CARROT, VEGETABLE_SOUP, APPLE, EGG, APPLE_PIE,
    GUN_POWDER, VESSEL, BOMB, PEAR, FRUIT_SALAD, MEAT, STEW, JAM, RUBY, GOLD_POWDER, DOLLAR, FRAME,
    DOLLAR_FRAME, RUBY_RING, COIN, FLIMSY_CROWN, STURDY_CROWN, EGGNOG
}

[System.Serializable]
public class ItemEntry{
    [SerializeField] public DungeonCrawlerTile type;
    [SerializeField] public int price;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Color color;
    [SerializeField] public string floorsItAppearsInStr;
    [NonSerialized] public List<int> floorsItAppearsIn;
    [SerializeField] public string toolRequiredStr;
    [NonSerialized] public List<ItemType> toolRequired;
};

[Serializable]
[CreateAssetMenu(fileName = "item_db", menuName = "ScriptableObjects/ItemDatabase", order = 1)]
public class ItemDatabase : ScriptableObject
{
    public SerializableDictionaryBase<ItemType, ItemEntry> list;

    public void Initialize(){
        list.ToList()
            .Where(p => !string.IsNullOrEmpty(p.Value.floorsItAppearsInStr)).ToList()
            .ForEach(p => p.Value.floorsItAppearsIn = p.Value.floorsItAppearsInStr
                                                                .Split(',').Select(s => int.Parse(s))
                                                                .ToList());
        list.ToList().Where(p => !string.IsNullOrEmpty(p.Value.floorsItAppearsInStr)).ToList()
            .ForEach(p => p.Value.toolRequired = p.Value.toolRequiredStr
                                                        .Split(',').Select(s => ParseTool(s))
                                                        .ToList());
    
    }

    private ItemType ParseTool(string s)
    {
        var list = new List<ItemType>();
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType))) {
            list.Add(itemType);
        }
        return list.Find(it => it.ToString() == s);
    }
}
