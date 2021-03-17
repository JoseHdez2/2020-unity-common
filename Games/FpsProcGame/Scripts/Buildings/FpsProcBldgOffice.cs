using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class FpsProcBldgOffice : FpsProcBldg {

    // bldgs > npcs > organizations
    // npcs > organizations > bldgs

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