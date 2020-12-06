using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class ProcFpsGeneratorBuilding : FpsProcArea {

    public List<List<string>> GenerateBuilding(Vector3Int gridSize){
        Vector2Int floorSize = new Vector2Int(gridSize.x, gridSize.y);
        List<List<string>> grid = CreateCube(gridSize, '.');
        // grid = grid.Select(floor => FillSquare(floor, new Vector2Int(0,0), new Vector2Int(2,2), '+')).ToList();
        // grid = grid.Select(floor => SetTile(floor, floorSize.RandomPos(), 'u')).ToList();
        for(int z = 0; z < gridSize.z; z++){
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

}