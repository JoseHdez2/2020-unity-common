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
    [SerializeField] public ItemType input1;
    [SerializeField] public ItemType input2;

    [SerializeField] public ItemType output;
    public int price;
};

[CreateAssetMenu(fileName = "recipes_", menuName = "ScriptableObjects/RecipeList", order = 1)]
public class CraftingRecipeList : ScriptableObject
{
    [SerializeField] public List<CraftingRecipe> recipes;
}
