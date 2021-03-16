

using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
public class BldgGenOffice : AbsBldgGenerator {
    public override string GenerateName(FpsProcBldgData input) => $"{FpsProcDatabase.streetNames.RandomItem()} Building";

    public override List<List<string>> GenerateTilemap(FpsProcBldgData input){
        Vector3Int gridSize = input.gridSize;
        BoundsInt floorBounds = new BoundsInt(){xMax=gridSize.x, yMax=gridSize.y};
        BoundsInt floorBoundsInner = new BoundsInt(){xMin=1, xMax=gridSize.x-1, yMin=1, yMax=gridSize.y-1};
        List<List<string>> grid = BldgGenExtensions.CreateCube(gridSize, '.');

        // grid = grid.Select(floor => FillSquare(floor, new Vector2Int(0,0), new Vector2Int(2,2), '+')).ToList(); // cubicles
        grid = grid.Select(floor => floor.FillSquare(floorBounds.TopWall(), '┬')) // outer walls
                    .Select(floor => floor.FillSquare(floorBounds.LeftWall(), '├'))
                    .Select(floor => floor.FillSquare(new BoundsInt(){xMin=gridSize.x-1, xMax=gridSize.x, yMax=gridSize.y-1}, '┤'))
                    .Select(floor => floor.FillSquare(new BoundsInt(){xMax=gridSize.x-1, yMin=gridSize.y-1, yMax=gridSize.y}, '┴'))
                    .Select(floor => floor.SetTile((Vector2Int)floorBounds.min, '┌'))
                    .Select(floor => floor.SetTile(new Vector2Int(floorBounds.max.x-1, floorBounds.min.y), '┐'))
                    .Select(floor => floor.SetTile(new Vector2Int(floorBounds.min.x, floorBounds.max.y-1), '└'))
                    .Select(floor => floor.SetTile((Vector2Int)floorBounds.max - Vector2Int.one, '┘'))
                    .ToList(); // outer walls
                    
        int last = grid.Count - 1;
        grid[last] = grid[last].Replace(new Dictionary<char, char>(){
            {'.', '_'}, {'┌', '╔'}, {'┐', '╗'}, {'└', '╚'}, {'┘', '╝'}, {'┬', '╤'}, {'├', '╟'}, {'┤', '╢'}, {'┴', '╧'}});
        for(int z = gridSize.z-1; z > 0; z--){
            List<Vector2Int> emptyTilesThisFloor = GetTilesWithChar(floor: grid[z], c: '.');
            emptyTilesThisFloor.AddRange(GetTilesWithChar(floor: grid[z], c: '_'));
            List<Vector2Int> emptyTilesLowerFloor = GetTilesWithChar(floor: grid[z-1], c: '.');
            List<Vector2Int> candidates = emptyTilesThisFloor.Where(c => emptyTilesLowerFloor.Contains(c)).ToList();
            if(!candidates.IsEmpty()){
                Vector2Int randPos = candidates.RandomItem();
                grid[z] = grid[z].SetTile(randPos, z == gridSize.z-1 ? 'D' : 'd');
                grid[z-1] = grid[z-1].SetTile(randPos, 'u');
            } else {
                Debug.LogError("No candidates for stairs!");
            }
        }
        Debug.Log(TilemapToStr(grid, 0));
        Debug.Log(gridSize);
        grid[0] = grid[0].SetTile(pos: GetTilesWithChar(floor: grid[0], c: '┴').RandomItem(), c: '_'); // entrance

        return grid;
    }

    public string TilemapToStr(List<List<string>> tilemap, int floorNum) => string.Join("\n\n", Enumerable.Reverse(tilemap)
        .Select((f, i) => $"<color={((tilemap.Count - 1 - i) == floorNum ? "yellow" : "white")}>{string.Join("\n", f)}</color>"));

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