using System;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class ReDungLevelInterpreter : MonoBehaviour
{
    public DungeonCrawlerDatabase db;
    public DungCrawLevel[] levels;
    public int curLevelIndex = 0;
    private string curLevelStr;
    private DungeonCrawlerTile[][] curLevel;

    public int curLevelPlayerMaxHp = 100;
    private Vector2 levelOffset; // unused, but cool
    public Camera minimapCamera;
    public float minimapCamHeight;

    public GameObject playerPrefab;
    private DungeonCrawlerTile stairsToSpawnIn;

    private LevelLoader levelLoader;

    // Start is called before the first frame update
    [ExecuteInEditMode]
    private void Start(){
        levelLoader = FindObjectOfType<LevelLoader>();
        InitLevel();
    }

    public void InitLevel(){
        curLevelStr = levels[curLevelIndex].level;
        curLevel = ParseCurLevelStr(curLevelStr);
        // curLevelPlayerMaxHp = levels[curLevelIndex].playerMaxHealth;
        transform.DeleteAllChildren();
        string[] rows = curLevelStr.Split('\n');
        Vector3 levelOffset3d = LevelOffset3d();
        for (int i = 0; i < rows.Length; i++) {
            for (int j = 0; j < rows[i].Length; j++) {
                DungeonCrawlerTile tileType = db.charToTile[rows[i][j]];
                if (tileType != DungeonCrawlerTile.NONE) {
                    Instantiate(db.tileToPrefab[tileType], MatrixPosToWorldPos(new Vector2Int(j, i)), Quaternion.identity, transform);
                }
            }
        }
        minimapCamera.transform.position = MatrixPosToWorldPos(new Vector2Int(curLevel.Width() / 2, curLevel.Height() / 2));
        minimapCamera.transform.position += Vector3.up * minimapCamHeight;
        SpawnPlayer();
    }

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
        Instantiate(playerPrefab, MatrixPosToWorldPos(freePos), Quaternion.identity, transform);
    }

    private Vector3 MatrixPosToWorldPos(Vector2Int matPos) 
        => new Vector3(matPos.x, 0, matPos.y) + LevelOffset3d();

    private Vector3 LevelOffset3d() => new Vector3(levelOffset.x, 0, levelOffset.y);

    private void OnGUI(){
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        GUI.skin.textArea = style;
    }

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