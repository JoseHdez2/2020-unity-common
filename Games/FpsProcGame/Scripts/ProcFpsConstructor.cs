using System;
using System.Collections.Generic;
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

    public void InstantiateTilemap(Vector3 origin, Vector3Int gridSize, Vector3 cellScale, List<List<string>> tilemap, string name = ""){
        GameObject parent = new GameObject(name);
        parent.transform.position = origin;
        for(int y = 0; y < gridSize.y; y++){ // floors
            for(int z = 0; z < gridSize.z; z++){
                for(int x = 0; x < gridSize.z; x++){
                    ProcFpsPrefab pf;
                    prefabs.TryGetValue(tilemap[z][y][x], out pf);
                    if(pf == null){
                        Debug.LogError($"No prefab for character '{tilemap[y][z][x]}'.");
                    } else {
                        Instantiate(pf.prefab, origin + new Vector3(y, z, x).ScaleWith(cellScale), Quaternion.Euler(pf.eulerAngles), parent.transform);
                    }
                }
            }
        }
    }
}