using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class FpsProcDistrict : MonoBehaviour {
    
    [TextArea(5,10)]
    public string tilemap;
    public SerializableDictionaryBase<char, FpsProcBldg> prefabs;
    [SerializeField] private Vector3Int cellScale;

    public void Start(){
        GenerateAndInstantiateBuildings();
    }

    public void GenerateAndInstantiateBuildings(){
        string[] rows = tilemap.Split('\n');
        for(int y = 0; y < rows.Length; y++){
            for(int x = 0; x < rows[y].Length; x++){
                FpsProcBldg pf;
                prefabs.TryGetValue(rows[y][x], out pf);
                if(pf == null){
                    Debug.LogError($"No prefab for character '{rows[y][x]}'.");
                } else {
                    FpsProcBldg bldg = Instantiate(pf, Vector3.zero, Quaternion.identity, transform);
                    bldg.data.origin = new Vector3(y, 0, x).ScaleWith(cellScale);
                    GenerateAndInstantiate(bldg);
                }
            }
        }
    }

    private void GenerateAndInstantiate(FpsProcBldg building){
        building.Generate();
        Debug.Log($"something: {building.data.TilemapToStr()}");
        building.Instantiate(FindObjectOfType<FpsProcGameManager>().pfNpc);
    }

}