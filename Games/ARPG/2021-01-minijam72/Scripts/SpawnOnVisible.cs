using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Spawn something when this is inside camera bounds. Note: OnBecameVisible requires a Renderer component.
/// </summary>
public class SpawnOnVisible : MonoBehaviour
{
    [SerializeField] GameObject objToSpawn;

    // // Update is called once per frame
    // void Update() {
    //     if(this)
    // }
    void OnBecameVisible()
    {
        Instantiate(objToSpawn, transform.position, Quaternion.identity);
    }
}
