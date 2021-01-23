using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MyTilemap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Tilemap tm = GetComponent<Tilemap>();
        tm.color = Color.white;
    }
}
