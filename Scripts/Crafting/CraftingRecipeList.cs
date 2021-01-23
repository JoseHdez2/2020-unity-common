using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class CraftingRecipe
{
    [SerializeField] public RpgCrawlerItemType input1;
    [SerializeField] public RpgCrawlerItemType input2;

    [SerializeField] public RpgCrawlerItemType output;
    public int price;
};

[CreateAssetMenu(fileName = "recipes_", menuName = "ScriptableObjects/RecipeList", order = 1)]
public class CraftingRecipeList : ScriptableObject
{
    [SerializeField] public List<CraftingRecipe> recipes;
}
