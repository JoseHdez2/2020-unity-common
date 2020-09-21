using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ExtensionMethods;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class CraftingController : MonoBehaviour
{
    private List<int> selectedItems = new List<int>(2);

    //public ItemEntry itemEntry;
    public ItemDatabase itemDatabase;
    public CraftingRecipeList craftingRecipeList;
    public ButtonMenu buttonMenuIngredients;
    public ButtonMenu buttonMenu;
    public int townSceneBuildIndex;
    private CraftingSystem craftingSystem = new CraftingSystem();
    private bool canCraft = false;
    private CraftingRecipe result = null;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Carfting menu started!");
        buttonMenu = FindObjectOfType<ButtonMenu>();

        //just for testing
        PlayerInventory.playerInventory.Add(ItemType.COPPER_ORE);
        PlayerInventory.playerInventory.Add(ItemType.OAK_WOOD);
        PlayerInventory.playerInventory.Add(ItemType.APPLE);

        result = craftingSystem.FindRecipe(craftingRecipeList, ItemType.COPPER_ORE, ItemType.OAK_WOOD);
        UpdateButtonMenus(false);        
    }

    void ToggleItemIndex(int index)
    {
        canCraft = false;
        // removes items previously selected (and resets craft button text and interaction)
        if (selectedItems.IndexOf(index) != -1)
            { selectedItems.RemoveAt(selectedItems.IndexOf(index)); canCraft = false;
            UpdateButtonMenus(false); /*buttonMenuIngredients.GetButtons()[0].interactable = false;*/ return; }
        if (selectedItems.Count >= 2) { return; }
        Debug.Log("Item added");
        selectedItems.Add(index);
        UpdateButtonMenus(false);
        // enables craft button when enough ingredients are selected (and obtains recipe result)
        if (selectedItems.Count >= 2) { canCraft = true; result =
                craftingSystem.FindRecipe(craftingRecipeList, selectedItems[0], selectedItems[1]); }        
        Debug.Log($"Result {result}");
        // displays result (in advance) inside the craft button
        if (canCraft && (result != null)) { UpdateButtonMenus(true); }
    }

    public void Craft()
    {
        if (canCraft) {
            craftingSystem.AttemptCraft(craftingRecipeList,
            selectedItems[0], selectedItems[1]);
        }        
        buttonMenuIngredients.GetButtons()[0].interactable = false;        
        selectedItems.Clear();
        UpdateButtonMenus(false);
        selectedItems.Clear();        
    }

    private void ExitScene() { SceneManager.LoadScene(townSceneBuildIndex); }

    private void UpdateButtonMenus(bool interactbl)
    {
        string nme = interactbl && (result.output != null) ? $"Craft {result.output}" : "Craft";        
        var selected = PlayerInventory.playerInventory.Where((it, i) => selectedItems.Contains(i) ).ToList();
        var notSelected = PlayerInventory.playerInventory.Where((it, i) => !selectedItems.Contains(i) ).ToList();
        buttonMenu.DestroyButtons();
        buttonMenuIngredients.DestroyButtons();
        //selected.ForEach(i => buttonMenuIngredients.AddButton(new ButtonData{ name = i.ToString(), interactable = true,
        //    action = MyExtensions.NewEvent(() => this.ToggleItemIndex(PlayerInventory.playerInventory.IndexOf(i)))}));
        // TODO Do this without hardcoding it
        if (selected.Count >= 1) {
            buttonMenuIngredients.AddButton(new ButtonData { name = selected[0].ToString(), interactable = true,
                action = MyExtensions.NewEvent(() => this.ToggleItemIndex(PlayerInventory.playerInventory.IndexOf(selected[0]))) });
        } else {buttonMenuIngredients.AddButton(new ButtonData {name = " ", interactable = false, action = 
            MyExtensions.NewEvent(() => Craft()) }); }
        buttonMenuIngredients.AddButton(new ButtonData {name = "+", interactable = false, action = 
            MyExtensions.NewEvent(() => Craft()) });
        if (selected.Count == 2) {
            buttonMenuIngredients.AddButton(new ButtonData { name = selected[1].ToString(), interactable = true,
                action = MyExtensions.NewEvent(() => this.ToggleItemIndex(PlayerInventory.playerInventory.IndexOf(selected[1]))) });
        } else {buttonMenuIngredients.AddButton(new ButtonData {name = " ", interactable = false, action = 
            MyExtensions.NewEvent(() => Craft()) }); }
        buttonMenuIngredients.AddButton(new ButtonData {name = "=", interactable = false, action = 
            MyExtensions.NewEvent(() => Craft())});        
        notSelected.ForEach(i => buttonMenu.AddButton(new ButtonData{ name = i.ToString(), interactable = true,
            action = MyExtensions.NewEvent(() => this.ToggleItemIndex(PlayerInventory.playerInventory.IndexOf(i)))}));
        buttonMenuIngredients.AddButton(new ButtonData {name = nme, action = MyExtensions.NewEvent(() => this.Craft()),
                interactable = interactbl});
                buttonMenuIngredients.AddButton(new ButtonData {name = " ", interactable = false, action = 
            MyExtensions.NewEvent(() => Craft()) });
        buttonMenuIngredients.AddButton(new ButtonData {name = "Exit", interactable = true, action = 
            MyExtensions.NewEvent(() => ExitScene()) });
    }
}
