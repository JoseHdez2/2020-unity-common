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
    
    [SerializeField] public SrpgMapData data;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap tilemapSolid;

    public Dictionary<string, string> legend;
    public Dictionary<string, string> legendSolid;

    public Dictionary<string, SrpgUnitData> units;

    public SerializableDictionaryBase<string, Tile> tiles;
    public SerializableDictionaryBase<string, Sprite> sprites;
    public SrpgUnit pfSrpgUnit;

    // Start is called before the first frame update
    void Start()
    {
        data = JsonUtility.FromJson<SrpgMapData>(mapJsonFile.text);
        legend = BuildLegend(data.legend);
        units = data.units.ToDictionary(u => u.id);
        BuildLevel(data.map);
        FindObjectOfType<SrpgController>(includeInactive: true).gameObject.SetActive(true);
    }

    private Dictionary<string, string> BuildLegend(string legendStr){
        return legendStr.Split(',').ToList()
            .Select(part => part.Replace(" ", ""))
            .ToDictionary(part => part.Split('=')[0], part => part.Split('=')[1]);
    }

    private void BuildLevel(List<string> map)
    {
        tilemap.ClearAllTiles();
        tilemapSolid.ClearAllTiles();
        FindObjectsOfType<SrpgUnit>().ToList().ForEach(u => Destroy(u.gameObject));
        int y = 0;
        foreach (string row in map){
            int x = -1;
            foreach (char tileChar in row){
                x++;
                if(tileChar == ' ') continue;
                string name = legend[tileChar.ToString()];
                Tile tile = null;
                SrpgUnitData unitData = null;
                if(tiles.TryGetValue(name, out tile)){
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                } else if (units.TryGetValue(name, out unitData)) {
                    SrpgUnit newUnit = Instantiate(pfSrpgUnit, new Vector3(x + 0.5f, y + 0.5f), Quaternion.identity);
                    newUnit.SetData(unitData);
                } // else check if it's a GLOBAL tile / unit.
            }
            
            y++;
        }
    }
}

[System.Serializable]
public class SrpgMapData {
    [SerializeField] public List<string> map;
    [SerializeField] public List<SrpgUnitData> units;
    [SerializeField] public string legend;
}
