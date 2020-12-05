using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class ProcFpsGeneratorBuilding : MonoBehaviour {
    [SerializeField] GameObject pfRoomSmall;
    [SerializeField] GameObject pfStairs;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;

    public List<List<string>> GenerateBuilding(Vector3Int gridSize){

        List<List<string>> grid = CreateCube(gridSize, '.');
        grid = grid.Select(floor => FillSquare(floor, new Vector2Int(0,0), new Vector2Int(2,2), '+')).ToList();
        return grid;

        // var watch = System.Diagnostics.Stopwatch.StartNew();
        // Debug.Log("${watch.ElapsedTicks}");
    }

    private List<List<string>> CreateCube(Vector3Int gridSize, char c){
        return Enumerable.Range(1, gridSize.z).Select(z => 
            Enumerable.Range(1, gridSize.y).Select(y => new string(c, gridSize.x)).ToList()
        ).ToList();
    }

    private List<string> FillSquare(List<string> grid, Vector2Int topleft, Vector2Int size, char c){
        return grid.Select((row, i) => i.IsBetween(topleft.y, topleft.y + size.y) ? row.ReplaceAt(topleft.x, new string(c, size.x)) : row)
            .ToList();
    }

}