
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ToFoLevelInterpreter : AbsDunCraLevelInterpreter
{
    private List<Vector3Int> spentResources = new List<Vector3Int>();
    private static List<Vector3Int> spentGoldenApples = new List<Vector3Int>();

    public TMP_Text textInventory;

    protected void Start(){
        base.Start();
        UpdatePlayerInventoryText();
    }

    protected void Update(){
    }
    
    protected override void SpawnTile(Vector2Int gridPos, DungeonCrawlerTile tileType){
        switch (tileType) {
            case DungeonCrawlerTile.CHOP:
            case DungeonCrawlerTile.HARVEST:
            case DungeonCrawlerTile.MINE:
                if (!spentResources.Any(p => p.x == gridPos.x && p.y == gridPos.y && p.z == curLevelIndex)) {
                    Debug.Log($"{tileType}: x:{gridPos.x}, y:{gridPos.y}");
                    Instantiate(db.tileToPrefab[tileType], GridPosToWorldPos(gridPos), Quaternion.identity, transform);
                }
                break;
            case DungeonCrawlerTile.GOLDEN_APPLE:
                if (!IsGoldenAppleSpent(gridPos)) {
                    Instantiate(db.tileToPrefab[tileType], GridPosToWorldPos(gridPos), Quaternion.identity, transform);
                }
                break;
            default:
                Instantiate(db.tileToPrefab[tileType], GridPosToWorldPos(gridPos), Quaternion.identity, transform);
                break;
        }
    }

    private bool IsGoldenAppleSpent(Vector2Int gridPos)
        => spentGoldenApples.Any(p => p.x == gridPos.x && p.y == gridPos.y && p.z == curLevelIndex);

    private bool IsResourceSpent(Vector2Int gridPos)
        => spentResources.Any(p => p.x == gridPos.x && p.y == gridPos.y && p.z == curLevelIndex);

    public void MarkResourceAsSpent(Vector3 worldPos){
        Debug.Log($"Mark resource as spent (1/2): {worldPos}.");
        Vector3Int levelPos = WorldPosToGridPos(worldPos);
        Debug.Log($"Mark resource as spent (2/2): {levelPos}.");
        spentResources.Add(levelPos);
        UpdatePlayerInventoryText();
    }

    public void MarkGoldenAppleAsSpent(Vector3 worldPos){
        Vector3Int levelPos = WorldPosToGridPos(worldPos);
        spentResources.Add(levelPos);
        UpdatePlayerInventoryText();
    }

    private void UpdatePlayerInventoryText(){
        var inv = PlayerInventory.playerInventory;
        textInventory.text = $"Items: {inv.Count} <sub>/ {inv.Capacity}</sub>";
    }

}
