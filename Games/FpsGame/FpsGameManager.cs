using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsGameManager : MonoBehaviour
{
    [SerializeField] GameObject pfRoomSmall;
    [SerializeField] GameObject pfStairs;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;

    void Start()
    {
        List<List<string>> blueprint = FindObjectOfType<ProcFpsGeneratorBuilding>().GenerateBuilding(gridSize);
        FindObjectOfType<ProcFpsConstructor>().InstantiateTilemap(new Vector3(), gridSize, cellScale, blueprint);
        // FindObjectOfType<ProcFpsGeneratorBuilding>().CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }

}
