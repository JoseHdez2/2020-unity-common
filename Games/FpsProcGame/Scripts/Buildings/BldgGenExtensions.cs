
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

namespace ExtensionMethods {
    public static class BldgGenExtensions {

        public static List<List<string>> CreateCube(Vector3Int gridSize, char c){
            return Enumerable.Range(1, gridSize.z).Select(z => 
                Enumerable.Range(1, gridSize.y).Select(y => new string(c, gridSize.x)).ToList()
            ).ToList();
        }

        public static List<string> FillSquare(this List<string> grid, BoundsInt bounds, char c){
            return grid.Select((row, i) => i.IsBetweenMaxExclusive(bounds.yMin, bounds.yMax) ? row.ReplaceAt(bounds.xMin, new string(c, bounds.size.x)) : row)
                .ToList();
        }
        public static List<string> Replace(this List<string> grid, Dictionary<char, char> dict){
            foreach(char c in dict.Keys){
                grid = grid.Select((row, i) => row.Replace(c, dict[c])).ToList();
            }
            return grid;
        }
        public static List<string> Replace(this List<string> grid, char oldChar, char newChar){
            return grid.Select((row, i) => row.Replace(oldChar, newChar)).ToList();
        }

        public static List<string> DrawSquare(this List<string> grid, BoundsInt bounds, char c){
            grid[bounds.yMin] = grid[bounds.yMin].ReplaceAt(bounds.xMin, new string(c, bounds.size.x));
            grid[bounds.yMax] = grid[bounds.yMin].ReplaceAt(bounds.xMin, new string(c, bounds.size.x));
            for(int y = bounds.yMin; y < bounds.yMax; y++){
                grid[y] = grid[y].ReplaceAt(bounds.xMin, c).ReplaceAt(bounds.xMax, c);
            }
            return grid;
        }

        public static List<string> SetTile(this List<string> grid, Vector2Int pos, char c){
            return grid.Select((row, i) => i == pos.y ? row.ReplaceAt(pos.x, c) : row)
                .ToList();
        }

    }
}