using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class DatabaseGameCrafting : MonoBehaviour
{
    public TextAsset jsonFile;

    public CraftingGameItems items;
    public CraftingGameRecipes recipes;

    public SerializableDictionaryBase<string, Sprite> itemSprites;
    
    void Awake()
    {
        items = JsonUtility.FromJson<CraftingGameItems>(jsonFile.text);
        recipes = JsonUtility.FromJson<CraftingGameRecipes>(jsonFile.text);
        // foreach(RecipeCraftingGame recipe in recipes.recipes){
        //     Debug.Log($"inputs: {recipe.inputs.Count}");
        //     Debug.Log($"outputs: {recipe.outputs.Count}");
        // }
    }
}

[System.Serializable]
public class CraftingGameItems {
    public ItemTypeCraftingGame[] items;
}

[System.Serializable]
public class CraftingGameRecipes {
    public RecipeCraftingGame[] recipes;
}

[System.Serializable]
public class ItemTypeCraftingGame : ItemTypeBase {
    public int price;
}

[System.Serializable]
public class RecipeCraftingGame {
    public string id;
    public string name;
    public List<ItemStackable> inputs;
    public List<ItemStackable> outputs;
    public int productionTime = 1;
}