using System;
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
    public bool autoInstantiate;
    [SerializeField] private FpsProcNpc pfNpc;

    private void Start() {
        if(autoInstantiate){
            GenerateAndInstantiateBuildings();
        }
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
        UpdateStreetNames();
        FindObjectOfType<FpsProcGameMgr>().RepositionNpcs();
    }

    private void UpdateStreetNames()
    {
        List<FpsProcBldgData> bldgs = GetComponentsInChildren<FpsProcBldg>()
            .Where( bldg => bldg.data.type == FpsProcBldgData.Type.STREET)
            .Select( bldg => bldg.data ).ToList(); // iterate through all prefabs, get the ones that are streets.
        
        // throw new NotImplementedException();
    }

    private void GenerateAndInstantiate(FpsProcBldg building){
        building.Generate();
        if(pfNpc == null){
            pfNpc = FindObjectOfType<FpsProcGameMgr>().pfNpc;
        }
        building.InstantiateBuilding(pfNpc);
    }

}