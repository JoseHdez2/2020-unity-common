using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Spawn something when this is inside camera bounds. Note: OnBecameVisible requires a Renderer component.
/// </summary>
public class SpawnOnVisible : MonoBehaviour {
    [SerializeField] GameObject objToSpawn;
    public enum EMode { NORMAL, BOSS };
    public EMode mode;
    private GameObject spawnedObj;

    void OnBecameVisible() {
        spawnedObj = Instantiate(objToSpawn, transform.position, Quaternion.identity);
    }

    void OnBecameInvisible() {
        if(mode == EMode.BOSS){
            if(!spawnedObj){ // If instance of boss was destroyed (boss was defeated)
                Destroy(this); // Don't spawn anymore.
            }
        }
    }
}
