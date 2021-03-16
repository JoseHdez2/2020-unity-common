using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

[System.Serializable]
public class FpsProcBldgData {
    
    public Vector3 origin;
    public Vector3 cellScale = Vector3.one; // area instantiation input.
    public Vector3Int gridSize;
    public string name = "";
    public List<List<string>> tilemap; // generation output. (instantiation input).
    public List<string> orgUuids;

    public int GetNumOfFloors() => gridSize.z;

    public Vector3 GetOrigin() => origin - cellScale / 2; // TODO fix this misalignment.

    public Vector3 GetSize() => gridSize.ScaleWith(cellScale);
    public Vector3 GetFloorSize() => new Vector3(gridSize.x, 1, gridSize.y).ScaleWith(cellScale);
    public Vector3 GetCenter() => GetOrigin() + GetSize() / 2;
    public Vector3 GetFloorCenter(int floorNum) => GetOrigin() + Vector3.up * cellScale.y * (floorNum + 0.5f) + GetFloorSize() / 2;
    public Bounds GetBounds() => new Bounds(GetCenter(), GetSize());
    public Bounds GetFloorBounds(int floorNum) => new Bounds(GetFloorCenter(floorNum), GetFloorSize());

    public string TilemapToStr() => string.Join("\n\n", Enumerable.Reverse(tilemap).Select((f,i) => $"F{tilemap.Count - i}:\n{string.Join("\n", f)}"));
    public string TilemapToStr(int floorNum) => string.Join("\n\n", Enumerable.Reverse(tilemap)
        .Select((f, i) => $"<color={((tilemap.Count - 1 - i) == floorNum ? "yellow" : "white")}>{string.Join("\n", f)}</color>"));

    public string TilemapToStr2(int floorNum) => $"N\n\n{string.Join("\n", tilemap[floorNum])}\n\nS";
}

public abstract class FpsProcBldg : MonoBehaviour {
    public FpsProcBldgData data;
    public FpsProcBounds pfBounds;
    public List<CharToPrefabDict> prefabDicts;
    public AbsBldgGenerator tilemapGenerator;
    private FpsProcDatabase db; // names and images

    private void Awake() {
        db = FindObjectOfType<FpsProcDatabase>();
    }

    public void Generate(){
        db = FindObjectOfType<FpsProcDatabase>();
        db.Initialize();
        data.tilemap = tilemapGenerator.GenerateTilemap(data);
        data.name = tilemapGenerator.GenerateName(data);
        FpsProcGameMgr gameMgr = FindObjectOfType<FpsProcGameMgr>();
        if(gameMgr){
            data.orgUuids = Enumerable.Range(0, data.gridSize.z).Select(i => gameMgr.affiliationsMgr.organizations.RandomItem().name).ToList();
        }
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
    
    public void InstantiateBuilding(FpsProcNpc pfNpc){
        name = data.name;
        transform.position = data.origin;
        IDictionary<char, ProcFpsPrefab> prefabs = prefabDicts
            .Select(d => (IDictionary<char, ProcFpsPrefab>)d.prefabs)
            .Aggregate((a,b) => a.Merge(b));

        for(int z = 0; z < data.GetNumOfFloors(); z++){ // floors
            InstantiateFloor(z, prefabs);
        }
    }
    
    public void InstantiateFloor(int z, IDictionary<char, ProcFpsPrefab> prefabs){
        GameObject floor = new GameObject(name: $"Floor {z}");
        floor.transform.parent = transform;

        for (int y = 0; y < data.gridSize.y; y++){
            for (int x = 0; x < data.gridSize.x; x++){
                ProcFpsPrefab pf;
                prefabs.TryGetValue(data.tilemap[z][y][x], out pf);
                if (pf == null){
                    Debug.LogError($"No prefab for character '{data.tilemap[z][y][x]}'.");
                } else {
                    Instantiate(pf.prefab, data.origin + new Vector3(y, z, x).ScaleWith(data.cellScale), Quaternion.Euler(pf.eulerAngles), floor.transform);
                }
            }
        }
        InstantiateBoundsForFloor(z, floor);
    }

    private void InstantiateBoundsForFloor(int z, GameObject floor){
        FpsProcBounds bounds = Instantiate(pfBounds, data.GetFloorCenter(z), Quaternion.identity, floor.transform);
        bounds.transform.localScale = data.GetFloorSize().ScaleWith(new Vector3(0.95f, 0.2f, 0.95f));
        bounds.bldg = this;
        bounds.floorNum = z;

        FpsProcGameMgr gameMgr = FindObjectOfType<FpsProcGameMgr>();
        if (gameMgr){
            FpsProcOrganization randomOrg = gameMgr.affiliationsMgr.organizations.RandomItem();
            randomOrg.areas.Add(bounds);
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
    protected List<string> Replace(List<string> grid, Dictionary<char, char> dict){
        foreach(char c in dict.Keys){
            grid = grid.Select((row, i) => row.Replace(c, dict[c])).ToList();
        }
        return grid;
    }
    protected List<string> Replace(List<string> grid, char oldChar, char newChar){
        return grid.Select((row, i) => row.Replace(oldChar, newChar)).ToList();
    }

    protected List<string> DrawSquare(List<string> grid, BoundsInt bounds, char c){
        grid[bounds.yMin] = grid[bounds.yMin].ReplaceAt(bounds.xMin, new string(c, bounds.size.x));
        grid[bounds.yMax] = grid[bounds.yMin].ReplaceAt(bounds.xMin, new string(c, bounds.size.x));
        for(int y = bounds.yMin; y < bounds.yMax; y++){
            grid[y] = grid[y].ReplaceAt(bounds.xMin, c).ReplaceAt(bounds.xMax, c);
        }
        return grid;
    }

    protected List<string> SetTile(List<string> grid, Vector2Int pos, char c){
        return grid.Select((row, i) => i == pos.y ? row.ReplaceAt(pos.x, c) : row)
            .ToList();
    }
}