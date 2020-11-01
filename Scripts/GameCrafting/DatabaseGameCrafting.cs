using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class ItemTypeCraftingGame : ItemTypeBase {
    public Sprite sprite;
    public int price;
}

[System.Serializable]
public class CraftingGameItems {
    public ItemTypeCraftingGame[] items;
}

[System.Serializable]
public class RecipeCraftingGame {
    public string uuid;
    public int id;
    public string name;
    public Dictionary<string, int> inputs;
    public Dictionary<string, int> outputs;
}

[System.Serializable]
public class CraftingGameRecipes {
    public RecipeCraftingGame[] recipes;
}


public class DatabaseGameCrafting : MonoBehaviour
{
    public TextAsset jsonFile;

    public CraftingGameItems items;
    public CraftingGameRecipes recipes;

    public SerializableDictionaryBase<string, Sprite> itemSprites;
    
    void Start()
    {
        items = JsonUtility.FromJson<CraftingGameItems>(jsonFile.text);
        recipes = JsonUtility.FromJson<CraftingGameRecipes>(jsonFile.text);
        foreach(RecipeCraftingGame recipe in recipes.recipes){
            Debug.Log($"inputs: {recipe.inputs.Count}");
            Debug.Log($"outputs: {recipe.outputs.Count}");
        }
    }
}
