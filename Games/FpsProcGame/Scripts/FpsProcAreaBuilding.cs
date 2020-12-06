using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class FpsProcAreaBuilding : FpsProcArea {

    public override FpsProcAreaData GenerateArea(Vector3Int? gridSize){
        string name = $"{FpsProcDatabase.streetNames.RandomItem()} Building";
        Debug.Log(name);
        return new FpsProcAreaData(){name=name, tilemap=GenerateBuilding(gridSize.Value)};
    }

    public List<List<string>> GenerateBuilding(Vector3Int gridSize){
        BoundsInt floorBounds = new BoundsInt(){xMax=gridSize.x, yMax=gridSize.y};
        BoundsInt floorBoundsInner = new BoundsInt(){xMin=1, xMax=gridSize.x-1, yMin=1, yMax=gridSize.y-1};
        List<List<string>> grid = CreateCube(gridSize, '.');
        // grid = grid.Select(floor => FillSquare(floor, new Vector2Int(0,0), new Vector2Int(2,2), '+')).ToList(); // cubicles
        grid = grid.Select(floor => OutlineSquare(floor, floorBounds, '|')).ToList(); // outer walls
        // grid = grid.Select(floor => SetTile(floor, floorSize.RandomPos(), 'u')).ToList();
        for(int z = 0; z < gridSize.z; z++){
            Vector2Int randPos;
            do {
                randPos = floorBoundsInner.RandomPos();
            } while(grid[z][randPos.y][randPos.x] != '.'); // randPos must be empty! // TODO avoid infinite checking.
            // TODO avoid placing stairs exactly on top of the previous ones.
            grid[z] = SetTile(grid[z], randPos, 'u');
            if(z < gridSize.z - 1) {
                grid[z+1] = SetTile(grid[z], randPos, 'd');
            }
            grid[z] = SetTile(grid[z], (Vector2Int)floorBounds.min, '┌');
            grid[z] = SetTile(grid[z], (Vector2Int)floorBounds.max, '┘');
        }
        return grid;

        // var watch = System.Diagnostics.Stopwatch.StartNew();
        // Debug.Log("${watch.ElapsedTicks}");
    }
}