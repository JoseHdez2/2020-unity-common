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
        GameObject parent = new GameObject(name: d.name);
        parent.transform.position = d.origin;
        for(int y = 0; y < d.gridSize.y; y++){ // floors
            for(int z = 0; z < d.gridSize.z; z++){
                for(int x = 0; x < d.gridSize.z; x++){
                    ProcFpsPrefab pf;
                    prefabs.TryGetValue(d.tilemap[z][y][x], out pf);
                    if(pf == null){
                        Debug.LogError($"No prefab for character '{d.tilemap[y][z][x]}'.");
                    } else {
                        Instantiate(pf.prefab, d.origin + new Vector3(y, z, x).ScaleWith(d.cellScale), Quaternion.Euler(pf.eulerAngles), parent.transform);
                    }
                }
            }
        }
    }
}