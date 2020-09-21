using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using System.Linq;

/// <summary>
/// InventoryMenu assumes it is in the root of a ButtonMenu gameobject.
/// It should work independently of layout (grid layout, list layout, etc).
/// </summary>
public class InventoryMenu : MonoBehaviour
{
    private ButtonMenu buttonMenu;
    private int selectedIndex;

    // Start is called before the first frame update
    void Awake()
    {
        buttonMenu = gameObject.GetComponent<ButtonMenu>();
    }

    private void OnEnable()
    {
        buttonMenu.enabled = false;
        buttonMenu.ClearButtonData();

        var pInv = PlayerInventory.playerInventory;
        for (int i = 0; i < pInv.Count; i++) {
            int j = i; // TODO SelectItem(i) doesn't work, but SelectItem(j) works perfectly. black magic.
            buttonMenu.AddButtonData(new ButtonData() { index = i, name = PlayerInventory.GetItemName(i), action = MyExtensions.NewEvent(() => SelectItem(j)) });
        }
        buttonMenu.enabled = true; // FIXME hacky code to avoid data races.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelectItem(int index)
    {
        // int index 
        selectedIndex = index;
        Debug.Log($"selectedIndex: {selectedIndex}");
    }    

    void DiscardItemAndRefreshMenu(int itemIndex) {
        PlayerInventory.DiscardItem(itemIndex);
        buttonMenu.enabled = false;
        buttonMenu.enabled = true;
    }
}
