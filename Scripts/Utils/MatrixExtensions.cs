using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class MatrixExtensions
    {
        public static Vector2Int? GetFirstInMatrix<T>(this T[][] matrix, T tile) {
            for (int y = 0; y < matrix.Height(); y++) {
                for (int x = 0; x < matrix[y].Length; x++) {
                    if (matrix[y][x].Equals(tile)) {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return null;
        }

        public static int Width<T>(this T[][] matrix) => matrix[0].Length;
        public static int Height<T>(this T[][] matrix) => matrix.Length;

        // "ToFourthQuadrantWorldPos" is too long.
        public static Vector3 ToWorldPos<T>(this Vector2Int pos, T[][] matrix)
            => new Vector3(pos.x, 0, pos.y);

        public static bool IsInsideMatrix<T>(this Vector2Int pos, T[][] matrix)
            => pos.x >= 0 && pos.x < matrix.Width() && pos.y >= 0 && pos.y < matrix.Height();

        public static Vector2Int[] AdjacentPositions<T>(this T[][] matrix, Vector2Int pos, bool includeDiagonals = true) {
            Vector2Int[] positions = new Vector2Int[] { pos.Up(), pos.Down(), pos.Left(), pos.Right() };
            if (includeDiagonals) {
                positions = positions.Concat(new Vector2Int[] { pos.Up().Left(), pos.Up().Right(), pos.Down().Left(), pos.Down().Right()});
            }
            return positions.Where(p => p.IsInsideMatrix(matrix)).ToArray();
        }

        public static T[] AdjacentTiles<T>(this T[][] matrix, Vector2Int pos, bool includeDiagonals = true)
            => matrix.AdjacentPositions(pos, includeDiagonals).Select(p => matrix.Get(p)).ToArray();
        
        public static T Get<T>(this T[][] matrix, Vector2Int pos) => matrix[pos.y][pos.x];

        public static Vector2Int Up(this Vector2Int v) => new Vector2Int(v.x, v.y + 1);
        public static Vector2Int Down(this Vector2Int v) => new Vector2Int(v.x, v.y - 1);
        public static Vector2Int Left(this Vector2Int v) => new Vector2Int(v.x - 1, v.y);
        public static Vector2Int Right(this Vector2Int v) => new Vector2Int(v.x + 1, v.y);
    }
}