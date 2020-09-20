
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ToFoLevelInterpreter : DunCraLevelInterpreter
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
    
    new protected void SpawnTile(int i, int j, DungeonCrawlerTile tileType){
        switch (tileType) {
            case DungeonCrawlerTile.CHOP:
            case DungeonCrawlerTile.HARVEST:
            case DungeonCrawlerTile.MINE:
                if (spentResources.Any(p => p.x == j && p.y == i && p.z == curLevelIndex)) {
                    Instantiate(db.tileToPrefab[tileType], MatrixPosToWorldPos(new Vector2Int(j, i)), Quaternion.identity, transform);
                }
                break;
            case DungeonCrawlerTile.GOLDEN_APPLE:
                if (!IsGoldenAppleSpent(i, j)) {
                    Instantiate(db.tileToPrefab[tileType], MatrixPosToWorldPos(new Vector2Int(j, i)), Quaternion.identity, transform);
                }
                break;
            case DungeonCrawlerTile.PLAYER:
                GameObject player = Instantiate(db.tileToPrefab[tileType], MatrixPosToWorldPos(new Vector2Int(j, i)), playerRotation, transform);
                break;
            default:
                Instantiate(db.tileToPrefab[tileType], MatrixPosToWorldPos(new Vector2Int(j, i)), Quaternion.identity, transform);
                break;
        }
    }

    private bool IsGoldenAppleSpent(int i, int j)
        => spentGoldenApples.Any(p => p.x == j && p.y == i && p.z == curLevelIndex);

    private bool IsResourceSpent(int i, int j)
        => spentResources.Any(p => p.x == j && p.y == i && p.z == curLevelIndex);

    public void MarkResourceAsSpent(Vector3 worldPos){
        Vector3Int levelPos = WorldPosToLevelPos(worldPos);
        Debug.Log($"Mark resource as spent: {worldPos}.");
        spentResources.Add(levelPos);
        UpdatePlayerInventoryText();
    }

    private Vector3Int WorldPosToLevelPos(Vector3 worldPos)
            => new Vector3Int((int)worldPos.x, (int)worldPos.y, curLevelIndex);

    public void MarkGoldenAppleAsSpent(Vector3 worldPos){
        Vector3Int levelPos = WorldPosToLevelPos(worldPos);
        spentResources.Add(levelPos);
        UpdatePlayerInventoryText();
    }

    private void UpdatePlayerInventoryText(){
        var inv = PlayerInventory.playerInventory;
        textInventory.text = $"Items: {inv.Count} <sub>/ {inv.Capacity}</sub>";
    }

}
