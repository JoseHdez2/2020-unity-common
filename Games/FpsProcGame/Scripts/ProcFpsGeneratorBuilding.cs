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
        Vector2Int floorSize = new Vector2Int(gridSize.x, gridSize.y);
        List<List<string>> grid = CreateCube(gridSize, '.');
        grid = grid.Select(floor => FillSquare(floor, new Vector2Int(0,0), new Vector2Int(2,2), '+')).ToList();
        // grid = grid.Select(floor => SetTile(floor, floorSize.RandomPos(), 'u')).ToList();
        for(int z = 0; z < gridSize.z; z++){
            Debug.Log(z);
            var randPos = floorSize.RandomPos();
            grid[z] = SetTile(grid[z], randPos, 'u');
            if(z < gridSize.z - 1) {
                grid[z+1] = SetTile(grid[z], randPos, 'd');
            }
        }
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
        return grid.Select((row, i) => i.IsBetweenMaxExclusive(topleft.y, topleft.y + size.y) ? row.ReplaceAt(topleft.x, new string(c, size.x)) : row)
            .ToList();
    }

    private List<string> SetTile(List<string> grid, Vector2Int pos, char c){
        return grid.Select((row, i) => i == pos.y ? row.ReplaceAt(pos.x, c) : row)
            .ToList();
    }

}