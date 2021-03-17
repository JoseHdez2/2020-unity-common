

using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public interface ITilemapGenerator3d {

    ///<summary>
    /// Generate the random tilemap, for later instantiation.
    /// Use ASCII and https://en.wikipedia.org/wiki/Box-drawing_character#DOS.
    ///  	│ 	┤ 	╡ 	╢ 	╖ 	╕ 	╣ 	║ 	╗ 	╝ 	╜ 	╛ 	┐
    /// └ 	┴ 	┬ 	├ 	─ 	┼ 	╞ 	╟ 	╚ 	╔ 	╩ 	╦ 	╠ 	═ 	╬ 	╧
    /// ╨ 	╤ 	╥ 	╙ 	╘ 	╒ 	╓ 	╫ 	╪ 	┘ 	┌ 	
    ///</summary>
    List<List<string>> GenerateTilemap(FpsProcBldgData input);
    string GenerateName(FpsProcBldgData input);

}

public abstract class AbsBldgGenerator : MonoBehaviour, ITilemapGenerator3d
{
    public abstract string GenerateName(FpsProcBldgData input);
    public abstract List<List<string>> GenerateTilemap(FpsProcBldgData input);
    public abstract FpsProcBldgData.Type GenerateType(FpsProcBldgData input);
}

[System.Serializable]
public class BldgGenerator : AbsBldgGenerator {
    public string bldgNameSuffix;
    public FpsProcBldgData.Type type;

    [SerializeField] public List<List<string>> tilemap;

    public override string GenerateName(FpsProcBldgData input) => $"{FpsProcDatabase.streetNames.RandomItem()} {bldgNameSuffix}";

    public override List<List<string>> GenerateTilemap(FpsProcBldgData input) => tilemap;

    public override FpsProcBldgData.Type GenerateType(FpsProcBldgData input) => type;
}