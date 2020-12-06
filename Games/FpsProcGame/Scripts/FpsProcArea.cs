using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

[System.Serializable]
public class FpsProcAreaData {
    public string name = "";
    public Vector3 origin;
    public Vector3Int gridSize;
    public Vector3 cellScale = Vector3.one;
    public List<List<string>> tilemap;
}

public abstract class FpsProcArea : MonoBehaviour {
    
    protected List<List<string>> CreateCube(Vector3Int gridSize, char c){
        return Enumerable.Range(1, gridSize.z).Select(z => 
            Enumerable.Range(1, gridSize.y).Select(y => new string(c, gridSize.x)).ToList()
        ).ToList();
    }

    protected List<string> FillSquare(List<string> grid, Vector2Int topleft, Vector2Int size, char c){
        return grid.Select((row, i) => i.IsBetweenMaxExclusive(topleft.y, topleft.y + size.y) ? row.ReplaceAt(topleft.x, new string(c, size.x)) : row)
            .ToList();
    }

    protected List<string> SetTile(List<string> grid, Vector2Int pos, char c){
        return grid.Select((row, i) => i == pos.y ? row.ReplaceAt(pos.x, c) : row)
            .ToList();
    }
}