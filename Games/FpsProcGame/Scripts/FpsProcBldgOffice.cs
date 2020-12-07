using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class FpsProcBldgOffice : FpsProcBldg {

    public override string GenerateName(FpsProcBldgData input) => $"{FpsProcDatabase.streetNames.RandomItem()} Building";

    public override List<List<string>> GenerateTilemap(FpsProcBldgData input){
        Vector3Int gridSize = input.gridSize;
        BoundsInt floorBounds = new BoundsInt(){xMax=gridSize.x, yMax=gridSize.y};
        BoundsInt floorBoundsInner = new BoundsInt(){xMin=1, xMax=gridSize.x-1, yMin=1, yMax=gridSize.y-1};
        List<List<string>> grid = CreateCube(gridSize, '.');
        // grid = grid.Select(floor => FillSquare(floor, new Vector2Int(0,0), new Vector2Int(2,2), '+')).ToList(); // cubicles
        grid = grid.Select(floor => FillSquare(floor, new BoundsInt(){yMax=1, xMax=gridSize.x-1}, '┬')) // outer walls
                    .Select(floor => FillSquare(floor, new BoundsInt(){xMax=1, yMax=gridSize.y-1}, '├'))
                    .Select(floor => FillSquare(floor, new BoundsInt(){xMin=gridSize.x-1, xMax=gridSize.x, yMax=gridSize.y-1}, '┤'))
                    .Select(floor => FillSquare(floor, new BoundsInt(){xMax=gridSize.x-1, yMin=gridSize.y-1, yMax=gridSize.y}, '┴'))
                    .Select(floor => SetTile(floor, (Vector2Int)floorBounds.min, '┌'))
                    .Select(floor => SetTile(floor, new Vector2Int(floorBounds.max.x-1, floorBounds.min.y), '┐'))
                    .Select(floor => SetTile(floor, new Vector2Int(floorBounds.min.x, floorBounds.max.y-1), '└'))
                    .Select(floor => SetTile(floor, (Vector2Int)floorBounds.max - Vector2Int.one, '┘'))
                    // .Select(floor => SetTile(floor, new Vector2Int(), 'x'))
                    .ToList(); // outer walls
        for(int z = 0; z < gridSize.z; z++){
            List<Vector2Int> emptyTilesThisFloor = GetTilesWithChar(floor: grid[z], c: '.');
            if(z+1 >= gridSize.z) { // if last floor...
                if(!emptyTilesThisFloor.IsEmpty()){
                    grid[z] = SetTile(grid[z], emptyTilesThisFloor.RandomItem(), 'u');
                }
            } else {
                List<Vector2Int> emptyTilesUpperFloor = GetTilesWithChar(floor: grid[z+1], c: '.');
                List<Vector2Int> candidates = emptyTilesThisFloor.Where(c => emptyTilesUpperFloor.Contains(c)).ToList();
                if(!candidates.IsEmpty()){
                    Vector2Int randPos = candidates.RandomItem();
                    grid[z] = SetTile(grid[z], randPos, 'u');
                    grid[z+1] = SetTile(grid[z], randPos, 'd');
                }
            }
        }

        return grid;
    }


    protected List<Vector2Int> GetTilesWithChar(List<string> floor, char c) =>
        floor.SelectMany((row, y) => row.Select((tile, x) => new Tile(){x=x, y=y, c=tile}))
                .Where(t => t.c == c)
                .Select(t => new Vector2Int(t.x,t.y))
            .ToList();

    protected struct Tile {
        public int x, y;
        public char c;
    }
}