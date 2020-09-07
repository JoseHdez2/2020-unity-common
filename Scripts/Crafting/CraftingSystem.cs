using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CraftingSystem
{
    public void AttemptCraft(CraftingRecipeList recipeList, int index1, int index2)
    {
        CraftingRecipe result = FindRecipe(recipeList, index1, index2);
        if (result == null) { Debug.Log("No valid recipe..."); return; }
        if (result.price > PlayerInventory.money) { Debug.Log("Not enough money..."); return; }
        PlayerInventory.playerInventory.RemoveAt(index1);
        if (index2 < index1) { PlayerInventory.playerInventory.RemoveAt(index2); }
            else { PlayerInventory.playerInventory.RemoveAt(index2 - 1); }
        PlayerInventory.money -= result.price;
        PlayerInventory.playerInventory.Add(result.output);
        Debug.Log($"Added {result.output}");

    }

    public CraftingRecipe FindRecipe(CraftingRecipeList recipeList, int index1, int index2)
    {
        ItemType item1 = PlayerInventory.playerInventory[index1];
        ItemType item2 = PlayerInventory.playerInventory[index2];
        return FindRecipe(recipeList, item1, item2);
    }

    public CraftingRecipe FindRecipe(CraftingRecipeList recipeList, ItemType item1, ItemType item2)
    {
        return recipeList.recipes.First(r => asd(r, item1, item2));
    }

    private bool asd(CraftingRecipe r, ItemType item1, ItemType item2)
    {
        return (r.input1 == item1 && r.input2 == item2) || (r.input1 == item2 && r.input2 == item1);
    }
}