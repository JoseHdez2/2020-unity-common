using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using ExtensionMethods;

[Serializable]
public class ProcFpsPrefab {
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 eulerAngles = new Vector3();
}

/// <summary>
/// Takes 3d "tilemaps" and instantiates them, translating each character to a prefab.
/// </summary>
public class ProcFpsConstructor : MonoBehaviour {
    public SerializableDictionaryBase<char, ProcFpsPrefab> prefabs;

    public void InstantiateTilemap(FpsProcAreaData d){
        GameObject building = new GameObject(name: d.name);
        building.transform.position = d.origin;
        for(int z = 0; z < d.gridSize.z; z++){ // floors
            GameObject floor = new GameObject(name: $"Floor {z}");
            floor.transform.parent = building.transform;
            for(int y = 0; y < d.gridSize.y; y++){
                for(int x = 0; x < d.gridSize.x; x++){
                    ProcFpsPrefab pf;
                    prefabs.TryGetValue(d.tilemap[z][y][x], out pf);
                    if(pf == null){
                        Debug.LogError($"No prefab for character '{d.tilemap[z][y][x]}'.");
                    } else {
                        Instantiate(pf.prefab, d.origin + new Vector3(y, z, x).ScaleWith(d.cellScale), Quaternion.Euler(pf.eulerAngles), floor.transform);
                    }
                }
            }
        }
    }
}