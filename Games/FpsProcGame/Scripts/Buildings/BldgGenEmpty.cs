using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class BldgGenEmpty : AbsBldgGenerator {
    public string bldgNameSuffix;
    
    public FpsProcBldgData.Type type;

    public List<List<string>> tilemap;

    public override string GenerateName(FpsProcBldgData input) => $"{FpsProcDatabase.streetNames.RandomItem()} {bldgNameSuffix}";

    public override List<List<string>> GenerateTilemap(FpsProcBldgData input) => BldgGenExtensions.CreateCube(input.gridSize, '.');

    public override FpsProcBldgData.Type GenerateType(FpsProcBldgData input) => type;
}