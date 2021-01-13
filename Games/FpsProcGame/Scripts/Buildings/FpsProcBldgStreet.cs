using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class FpsProcBldgStreet : FpsProcBldg
{
    public override string GenerateName(FpsProcBldgData input) => $"{FpsProcDatabase.streetNames.RandomItem()} Street";

    public override List<List<string>> GenerateTilemap(FpsProcBldgData input)
    {
        return new List<List<string>>(){new List<string>{"...", "...", "..."}};
    }
}