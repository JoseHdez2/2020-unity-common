using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class SrpgMap : MonoBehaviour
{
    [SerializeField] private TextAsset mapJsonFile;
    
    public static SrpgMapData data;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap tilemapSolid;

    public Dictionary<string, string> legend;
    public string solidTilesConfig;
    private List<string> solidTiles; // TODO delete this?

    public Dictionary<string, SrpgUnitData> units;

    public SerializableDictionaryBase<string, Tile> tiles;
    public SerializableDictionaryBase<string, Sprite> sprites;
    public SrpgUnit pfSrpgUnit;

    // Start is called before the first frame update
    void Start()
    {
        legend = BuildLegend(data.legend);
        units = data.units.ToDictionary(u => u.id);
        solidTiles = solidTilesConfig.Split(',').Select(part => part.Replace(" ", "")).ToList();
        BuildLevel(data.map);
        FindObjectOfType<SrpgController>(includeInactive: true).gameObject.SetActive(true);
        FindObjectOfType<SrpgController>(includeInactive: true).UpdateTeamsSoft();
    }

    private Dictionary<string, string> BuildLegend(string legendStr){
        return legendStr.Split(',')
            .Select(part => part.Replace(" ", ""))
            .ToDictionary(part => part.Split('=')[0], part => part.Split('=')[1]);
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            FindObjectOfType<LevelLoader>().LoadPrevLevel();
        }
    }

    private void BuildLevel(List<string> map)
    {
        tilemap.ClearAllTiles();
        tilemapSolid.ClearAllTiles();
        FindObjectsOfType<SrpgUnit>().ToList().ForEach(u => Destroy(u.gameObject));
        int y = map.Count() - 1 - (int)Math.Floor(map.Count() / 2f); // for centering;
        foreach (string row in map){
            int x = -1 - (int)Math.Floor(row.Length / 2f); // for centering
            foreach (char tileChar in row){
                x++;
                if(tileChar == ' ') continue;
                string name = legend[tileChar.ToString()];
                Tile tile = null;
                SrpgUnitData unitData = null;
                if(tiles.TryGetValue(name, out tile)){
                    if(solidTiles.Contains(name)){ 
                        tilemapSolid.SetTile(new Vector3Int(x, y, 0), tile);
                    } else {
                        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                } else if (units.TryGetValue(name, out unitData)) {
                    SrpgUnit newUnit = Instantiate(pfSrpgUnit, new Vector3(x + 0.5f, y + 0.5f), Quaternion.identity);
                    newUnit.SetData(unitData);
                } // else check if it's a GLOBAL tile / unit.
            }
            
            y--;
        }
    }
}

[System.Serializable]
public class SrpgMapData {
    [SerializeField] public List<string> map;
    [SerializeField] public List<SrpgUnitData> units;
    [SerializeField] public string legend;
}
