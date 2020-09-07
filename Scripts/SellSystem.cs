using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ExtensionMethods;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class SellSystem : MonoBehaviour
{
    private List<int> selectedItems = new List<int>(2);

    //public ItemEntry itemEntry;
    public ItemDatabase itemDatabase;
    public ButtonMenu buttonMenuIngredients;
    public ButtonMenu buttonMenu;
    public TMP_Text playerMoney;
    public int townSceneBuildIndex;
    private int benefits;
    private int itOffset;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Selling menu started!");

        //just for testing
        PlayerInventory.playerInventory.Add(ItemType.COPPER_ORE);
        PlayerInventory.playerInventory.Add(ItemType.OAK_WOOD);
        PlayerInventory.playerInventory.Add(ItemType.APPLE);

        playerMoney.text = $"Money: {PlayerInventory.money.ToString()}$";
        
        UpdateButtonMenus(false);
    }

    void ToggleItemIndex(int index)
    {
        // removes items previously selected (and resets sell button text and interaction)
        if (selectedItems.IndexOf(index) != -1)
        { selectedItems.RemoveAt(selectedItems.IndexOf(index)); if (selectedItems.Count >= 1)
            { UpdateButtonMenus(true); } else { UpdateButtonMenus(false); } calculateBenefits(); return; }
        Debug.Log("Item added");
        selectedItems.Add(index);
        calculateBenefits();
        UpdateButtonMenus(true);
    }

    void calculateBenefits()
    {
        benefits = 0;
        selectedItems.ForEach(i => benefits += itemDatabase.list[PlayerInventory.playerInventory[i]].price);
    }

    // NOT FINISHED YET
    public void Sell()
    {
        selectedItems.Sort();
        selectedItems.Reverse(); // for removing items from a list by index.
        selectedItems.ForEach(ind => SellItem(ind));
        selectedItems.Clear();
        UpdateButtonMenus(canSell: false);
        playerMoney.text = $"Money: {PlayerInventory.money.ToString()}$";
    }

    private void SellItem(int ind)
    {
        PlayerInventory.money += itemDatabase.list[PlayerInventory.playerInventory[ind]].price;
        PlayerInventory.playerInventory.RemoveAt(ind);
    }

    private int updateMinIndex(int i, int minIndex)
    {
        // adjusts index due to list reducing when you remove items from them
        itOffset++;
        if (selectedItems[i] < minIndex) { PlayerInventory.playerInventory.RemoveAt(selectedItems[i]);
            minIndex = selectedItems[i]; } else { PlayerInventory.playerInventory.RemoveAt
            (selectedItems[i] - itOffset); }
        return minIndex;
    }

    private void ExitScene() { SceneManager.LoadScene(townSceneBuildIndex); }

    private void UpdateButtonMenus(bool canSell)
    {
        string nme = canSell ? $"Sell\nfor {benefits}$" : "Sell";
        var selected = PlayerInventory.playerInventory.Where((it, i) => selectedItems.Contains(i)).ToList();
        var notSelected = PlayerInventory.playerInventory.Where((it, i) => !selectedItems.Contains(i)).ToList();
        Debug.Log($"selected: {string.Join(", ", selected)}");
        Debug.Log($"notSelected: {string.Join(", ", notSelected)}");
        buttonMenu.ClearButtons();
        buttonMenuIngredients.ClearButtons();
        selected.ForEach(i => buttonMenuIngredients.AddButton(new ButtonData{ name = i.ToString(), interactable = true,
            action = MyExtensions.NewEvent(() => this.ToggleItemIndex(PlayerInventory.playerInventory.IndexOf(i)))}));   
        notSelected.ForEach(i => buttonMenu.AddButton(new ButtonData{ name = i.ToString(), interactable = true,
            action = MyExtensions.NewEvent(() => this.ToggleItemIndex(PlayerInventory.playerInventory.IndexOf(i)))}));
        buttonMenuIngredients.AddButton(new ButtonData {name = nme, action = MyExtensions.NewEvent(() => this.Sell()),
                interactable = canSell});
        buttonMenuIngredients.AddButton(new ButtonData {name = " ", interactable = false, action = 
            MyExtensions.NewEvent(() => Sell()) });
        buttonMenuIngredients.AddButton(new ButtonData {name = "Exit", interactable = true, action = 
            MyExtensions.NewEvent(() => ExitScene()) });
    }
}
