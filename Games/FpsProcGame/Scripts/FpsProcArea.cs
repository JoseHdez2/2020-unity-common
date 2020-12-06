using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

[System.Serializable]
public class FpsProcAreaData {
    public string name = "";
    public Vector3 origin;
    public Vector3Int gridSize; // area generation input.
    public Vector3 cellScale = Vector3.one; // area instantiation input.
    public List<List<string>> tilemap; // generation output. (instantiation input).

    public Bounds GetBounds() {
        Vector3 size = gridSize.ScaleWith(cellScale);
        return new Bounds(origin + size / 2, size);
    }
}

public abstract class FpsProcArea : MonoBehaviour {
    
    private FpsProcDatabase db;
    private void Awake() {
        db = FindObjectOfType<FpsProcDatabase>();
    }

    ///<summary>
    /// Generate the random tilemap, for later instantiation.
    /// Use ASCII and https://en.wikipedia.org/wiki/Box-drawing_character#DOS.
    ///  	│ 	┤ 	╡ 	╢ 	╖ 	╕ 	╣ 	║ 	╗ 	╝ 	╜ 	╛ 	┐
    /// └ 	┴ 	┬ 	├ 	─ 	┼ 	╞ 	╟ 	╚ 	╔ 	╩ 	╦ 	╠ 	═ 	╬ 	╧
    /// ╨ 	╤ 	╥ 	╙ 	╘ 	╒ 	╓ 	╫ 	╪ 	┘ 	┌ 	
    ///</summary>
    public abstract FpsProcAreaData GenerateArea(Vector3Int? gridSize);
    protected List<List<string>> CreateCube(Vector3Int gridSize, char c){
        return Enumerable.Range(1, gridSize.z).Select(z => 
            Enumerable.Range(1, gridSize.y).Select(y => new string(c, gridSize.x)).ToList()
        ).ToList();
    }

    protected List<string> FillSquare(List<string> grid, BoundsInt bounds, char c){
        return grid.Select((row, i) => i.IsBetweenMaxExclusive(bounds.yMin, bounds.yMax) ? row.ReplaceAt(bounds.xMin, new string(c, bounds.size.x)) : row)
            .ToList();
    }

    protected List<string> OutlineSquare(List<string> grid, BoundsInt bounds, char c){
        return grid.Select((row, i) => i == bounds.yMin || i == bounds.yMax-1 ? row.ReplaceAt(bounds.xMin, new string(c, bounds.size.x)) : row)
            .ToList();
    }

    protected List<string> SetTile(List<string> grid, Vector2Int pos, char c){
        return grid.Select((row, i) => i == pos.y ? row.ReplaceAt(pos.x, c) : row)
            .ToList();
    }
}