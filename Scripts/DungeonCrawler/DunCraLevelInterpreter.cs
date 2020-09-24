using System;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public abstract class AbsDunCraLevelInterpreter : MonoBehaviour
{
    public DungeonCrawlerDatabase db;
    public DungCrawLevel[] levels;
    public int curLevelIndex = 0;
    private string curLevelStr;
    private DungeonCrawlerTile[][] curLevel;
    
    private Vector2 levelOffset; // unused, but cool
    public Camera minimapCamera;
    public float minimapCamHeight;

    public ItemDatabase itemDatabase;
    public GameObject playerPrefab;
    private DungeonCrawlerTile stairsToSpawnIn;

    private LevelLoader levelLoader;


    // Start is called before the first frame update
    // [ExecuteInEditMode]
    protected void Start(){
        levelLoader = FindObjectOfType<LevelLoader>();
        itemDatabase.Initialize();
        InitLevel();
    }

    public void InitLevel(){
        curLevelStr = levels[curLevelIndex].level;
        curLevel = ParseCurLevelStr(curLevelStr);
        // curLevelPlayerMaxHp = levels[curLevelIndex].playerMaxHealth;
        transform.DeleteAllChildren();
        for (int y = 0; y < curLevel.Length; y++) {
            for (int x = 0; x < curLevel[y].Length; x++) {
                DungeonCrawlerTile tileType = curLevel[y][x];
                if (tileType != DungeonCrawlerTile.NONE) {
                    SpawnTile(new Vector2Int(x, y), tileType);
                }
            }
        }
        minimapCamera.transform.position = GridPosToWorldPos(new Vector2Int(curLevel.Width() / 2, curLevel.Height() / 2));
        minimapCamera.transform.position += Vector3.up * minimapCamHeight;
        SpawnPlayer();
    }

    protected abstract void SpawnTile(Vector2Int gridPos, DungeonCrawlerTile tileType);

    private DungeonCrawlerTile[][] ParseCurLevelStr(string curLevelStr) =>
        curLevelStr.Split('\n').ToList()
                .Select(r => r.ToCharArray())
                .Select(charRow => charRow.Select(c => db.charToTile[c]).ToArray())
                .ToArray();

    private void SpawnPlayer(){
        Vector2Int? stairsPos = curLevel.GetFirstInMatrix(stairsToSpawnIn);
        if (!stairsPos.HasValue) Debug.LogWarning("Did not find stairs for Player to spawn adjacent to!");
        Vector2Int freePos = curLevel.AdjacentPositions(stairsPos.Value)
            .Where(p => curLevel.Get(p) == DungeonCrawlerTile.NONE).First();
        GameObject player = Instantiate(playerPrefab, GridPosToWorldPos(freePos), Quaternion.identity, transform);
    }

    // we invert gridPos.y because the array is top-to-bottom while Unity coordinates are bottom-to-top.
    protected Vector3 GridPosToWorldPos(Vector2Int gridPos)
        => new Vector3(gridPos.x, 0, -gridPos.y) + new Vector3(levelOffset.x, 0, levelOffset.y);


    protected Vector3 GridPosToWorldPos(Vector3Int gridPos) 
            => new Vector3(gridPos.x, 0, -gridPos.y) + new Vector3(levelOffset.x, 0, levelOffset.y);

    protected Vector3Int WorldPosToGridPos(Vector3 worldPos)
            => new Vector3Int((int)(worldPos.x - levelOffset.x), 0, (int)(-worldPos.y - levelOffset.y));

    public void LoadNextLevel(){
        curLevelIndex++;
        stairsToSpawnIn = DungeonCrawlerTile.STAIRS_UP;
        InitLevel();
    }

    public void LoadPrevLevel(){
        curLevelIndex--;
        stairsToSpawnIn = DungeonCrawlerTile.STAIRS_DOWN;
        if (curLevelIndex < 0) {
            levelLoader.LoadPrevLevel();
        } else {
            InitLevel();
        }
    }
}

public class DunCraLevelInterpreter : AbsDunCraLevelInterpreter {

    protected override void SpawnTile(Vector2Int gridPos, DungeonCrawlerTile tileType)
    {
        Instantiate(db.tileToPrefab[tileType], GridPosToWorldPos(gridPos), Quaternion.identity, transform);
    }
}