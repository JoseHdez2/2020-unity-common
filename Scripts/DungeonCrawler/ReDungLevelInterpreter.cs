using ExtensionMethods;
using UnityEngine;

public class ReDungLevelInterpreter : MonoBehaviour
{
    public DungeonCrawlerDatabase db;
    public DungCrawLevel[] levels;
    public static int curLevelIndex = 0;
    private string curLevel;
    public int curLevelPlayerMaxHp;
    private Vector2 levelOffset; // unused, but cool
    public Camera minimapCamera;

    // Start is called before the first frame update
    [ExecuteInEditMode]
    private void Start()
    {
        InitLevel();
    }

    public void InitLevel()
    {
        curLevel = levels[curLevelIndex].level;
        curLevelPlayerMaxHp = levels[curLevelIndex].playerMaxHealth;
        transform.DeleteAllChildren();
        string[] rows = curLevel.Split('\n');
        Vector3 levelOffset3d = LevelOffset3d();
        for (int i = 0; i < rows.Length; i++) {
            for (int j = 0; j < rows[i].Length; j++) {
                DungeonCrawlerTile tileType = db.charToTile[rows[i][j]];
                if (tileType != DungeonCrawlerTile.NONE) {
                    Instantiate(db.tileToPrefab[tileType], new Vector3(j, 0, rows.Length - i) + levelOffset3d, Quaternion.identity, transform);
                }
            }
        }
        minimapCamera.transform.position += levelOffset3d + new Vector3(rows[0].Length, 0, rows.Length) / 2;
    }

    private Vector3 LevelOffset3d() => new Vector3(levelOffset.y, 0, levelOffset.x);

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        GUI.skin.textArea = style;
    }

    // Update is called once per frame
    private void Update()
    { }

    public Vector3? GetPosition(DungeonCrawlerTile tile)
    {
        string[] rows = curLevel.Split('\n');
        for (int i = 0; i < rows.Length; i++) {
            for (int j = 0; j < rows[i].Length; j++) {
                DungeonCrawlerTile tileType = db.charToTile[rows[i][j]];
                if (tileType == tile) {
                    return new Vector3(j, 0, rows.Length - i) + LevelOffset3d();
                }
            }
        }
        return null;
    }

    public void LoadNextLevel()
    {
        curLevelIndex++;
        InitLevel();
    }
}