using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class FpsProcBldgStreet : FpsProcBldg
{
    public string bldgNameSuffix;
    public override string GenerateName(FpsProcBldgData input) => $"{FpsProcDatabase.streetNames.RandomItem()} {bldgNameSuffix}";

    public override List<List<string>> GenerateTilemap(FpsProcBldgData input)
    {
        Vector3Int gridSize = input.gridSize;
        BoundsInt floorBounds = new BoundsInt(){xMax=gridSize.x, yMax=gridSize.y};
        BoundsInt floorBoundsInner = new BoundsInt(){xMin=1, xMax=gridSize.x-1, yMin=1, yMax=gridSize.y-1};
        List<List<string>> grid = CreateCube(gridSize, '.');
        return grid;
    }
}