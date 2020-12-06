using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Debug.Log($"something: {string.Join("\n\n", blueprint.Select(f => string.Join("\n", f)))}");
        FpsProcAreaData d = new FpsProcAreaData(){origin=new Vector3(10,0,10), gridSize=gridSize, cellScale=cellScale, tilemap=blueprint};
        FindObjectOfType<ProcFpsConstructor>().InstantiateTilemap(d);
        // FindObjectOfType<ProcFpsGeneratorBuilding>().CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }


}
