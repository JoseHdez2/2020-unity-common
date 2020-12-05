using System;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

/// <summary>
/// Takes 3d "tilemaps" and instantiates them, translating each character to a prefab.
/// </summary>
public class ProcFpsConstructor : MonoBehaviour {
    public SerializableDictionaryBase<char, GameObject> prefabs;

    public void InstantiateTilemap(Vector3 origin, Vector3Int gridSize, Vector3 cellScale, List<List<string>> tilemap){
        for(int x = 0; x < gridSize.x; x++){
            for(int y = 0; y < gridSize.y; y++){
                for(int z = 0; z < gridSize.z; z++){
                    Instantiate(prefabs[tilemap[z][y][x]], new Vector3(origin.x + x * cellScale.x, origin.y + y * cellScale.y, origin.z + z * cellScale.z), Quaternion.identity);
                }
            }
        }
    }
}