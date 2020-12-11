using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class FpsProcBldgParkingLot : FpsProcBldgOffice
{
    public override string GenerateName(FpsProcBldgData input) => $"{FpsProcDatabase.streetNames.RandomItem()} Parking Lot";
}