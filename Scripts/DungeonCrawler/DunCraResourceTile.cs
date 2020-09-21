using ExtensionMethods;
using System.Linq;
using UnityEngine;

public class DunCraResourceTile : Expirable
{
    public DungeonCrawlerTile resourceType;
    public ItemDatabase itemDatabase;

    private ToFoLevelInterpreter levelInterpreter;
    private DialogueManager dialogManager;

    public void Start() {
        levelInterpreter = FindObjectOfType<ToFoLevelInterpreter>();
        dialogManager = FindObjectOfType<DialogueManager>();
    }

    public void PickUpByPlayer() {

        ItemType itemType = itemDatabase.GenerateItem(resourceType, levelInterpreter.curLevelIndex);
        var pInv = PlayerInventory.playerInventory;
        pInv.Add(itemType);
        levelInterpreter.MarkResourceAsSpent(transform.position);
        if (dialogManager) { dialogManager.WriteOneShot($"Got {PlayerInventory.GetItemName(pInv.Count-1)}."); }
        Expire();
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            PickUpByPlayer();
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            PickUpByPlayer();
        }
    }
}
