using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

[System.Serializable]
public class FpsProcBldgData {
    
    public Vector3 origin;
    public Vector3 cellScale = Vector3.one; // area instantiation input.
    public Vector3Int gridSize;
    public int npcAmount;
    public string name = "";
    public List<List<string>> tilemap; // generation output. (instantiation input).

    public Bounds GetBounds() {
        Vector3 size = gridSize.ScaleWith(cellScale);
        return new Bounds(origin + size / 2, size);
    }

    public string TilemapToStr() => string.Join("\n\n", Enumerable.Reverse(tilemap).Select((f,i) => $"F{tilemap.Count - i}:\n{string.Join("\n", f)}"));
    public string TilemapToStr(int floorNum) => string.Join("\n\n", Enumerable.Reverse(tilemap)
        .Select((f, i) => $"<color={(i == floorNum ? "yellow" : "white")}>{string.Join("\n", f)}</color>"));
}

public abstract class FpsProcBldg : MonoBehaviour {

    public FpsProcBldgData data;
    
    private FpsProcDatabase db;
    public List<FpsProcNpcData> npcsData;
    public List<FpsProcNpc> npcs;

    private void Awake() {
        db = FindObjectOfType<FpsProcDatabase>();
    }

    public void Generate(){
        data.tilemap = GenerateTilemap(data);
        data.name = GenerateName(data);
    }
    
    ///<summary>
    /// Generate the random tilemap, for later instantiation.
    /// Use ASCII and https://en.wikipedia.org/wiki/Box-drawing_character#DOS.
    ///  	│ 	┤ 	╡ 	╢ 	╖ 	╕ 	╣ 	║ 	╗ 	╝ 	╜ 	╛ 	┐
    /// └ 	┴ 	┬ 	├ 	─ 	┼ 	╞ 	╟ 	╚ 	╔ 	╩ 	╦ 	╠ 	═ 	╬ 	╧
    /// ╨ 	╤ 	╥ 	╙ 	╘ 	╒ 	╓ 	╫ 	╪ 	┘ 	┌ 	
    ///</summary>
    public abstract List<List<string>> GenerateTilemap(FpsProcBldgData input);
    public abstract string GenerateName(FpsProcBldgData input);
    
    public void Instantiate(FpsProcNpc pfNpc){
        name = data.name;
        transform.position = data.origin;
        IDictionary<char, ProcFpsPrefab> prefabs = FindObjectOfType<ProcFpsConstructor>().prefabs;
        for(int z = 0; z < data.gridSize.z; z++){ // floors
            GameObject floor = new GameObject(name: $"Floor {z}");
            floor.transform.parent = transform;
            for(int y = 0; y < data.gridSize.y; y++){
                for(int x = 0; x < data.gridSize.x; x++){
                    ProcFpsPrefab pf;
                    prefabs.TryGetValue(data.tilemap[z][y][x], out pf);
                    if(pf == null){
                        Debug.LogError($"No prefab for character '{data.tilemap[z][y][x]}'.");
                    } else {
                        Instantiate(pf.prefab, data.origin + new Vector3(y, z, x).ScaleWith(data.cellScale), Quaternion.Euler(pf.eulerAngles), floor.transform);
                    }
                }
            }
        }
        
        var npcsParent = new GameObject("npcs");
        npcsParent.transform.parent = transform;
        Bounds areaBounds = data.GetBounds();
        npcs = new List<FpsProcNpc>();
        npcsData = new List<FpsProcNpcData>();
        Debug.Log(npcsData);
        foreach(int i in Enumerable.Range(0, data.npcAmount)){
            FpsProcNpc npc = Instantiate(pfNpc, areaBounds.RandomPos(), Quaternion.identity, npcsParent.transform);
            npcsData.Add(npc.data);
            npcs.Add(npc);
        }
    }

    protected List<List<string>> CreateCube(Vector3Int gridSize, char c){
        return Enumerable.Range(1, gridSize.z).Select(z => 
            Enumerable.Range(1, gridSize.y).Select(y => new string(c, gridSize.x)).ToList()
        ).ToList();
    }

    protected List<string> FillSquare(List<string> grid, BoundsInt bounds, char c){
        return grid.Select((row, i) => i.IsBetweenMaxExclusive(bounds.yMin, bounds.yMax) ? row.ReplaceAt(bounds.xMin, new string(c, bounds.size.x)) : row)
            .ToList();
    }

    protected List<string> SetTile(List<string> grid, Vector2Int pos, char c){
        return grid.Select((row, i) => i == pos.y ? row.ReplaceAt(pos.x, c) : row)
            .ToList();
    }
}