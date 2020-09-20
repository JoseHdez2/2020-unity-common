using ExtensionMethods;
using System.Linq;
using UnityEngine;

public class DunCraResourceTile : Expirable
{
    public DungeonCrawlerTile resourceType;
    public ItemDatabase itemDatabase;

    private AbsDunCraLevelInterpreter levelInterpreter;
    private DialogueManager dialogManager;

    public void Start() {
        levelInterpreter = FindObjectOfType<AbsDunCraLevelInterpreter>();
        dialogManager = FindObjectOfType<DialogueManager>();
    }

    public void PickUpByPlayer() {
        // levelInterpreter.MarkResourceAsSpent(transform.position);

        ItemType itemType = GenerateItem(resourceType, levelInterpreter.curLevelIndex);
        PlayerInventory.playerInventory.Add(itemType);
        // levelInterpreter.
        if (dialogManager) { dialogManager.WriteOneShot($"Got {itemType}."); }
        Expire();
    }

    private ItemType GenerateItem(DungeonCrawlerTile resourceType, int levelDepth)
        => itemDatabase.list.ToList()
            .Where(p => p.Value.floorsItAppearsIn.Contains(levelDepth))
            .ToList().RandomItem().Key;

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
