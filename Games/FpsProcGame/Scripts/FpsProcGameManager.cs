using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FpsProcGameManager : MonoBehaviour
{
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;

    void Start()
    {
        FpsProcAreaData blueprint = FindObjectOfType<FpsProcAreaBuilding>().GenerateArea(gridSize);
        Debug.Log($"something: {string.Join("\n\n", blueprint.tilemap.Select(f => string.Join("\n", f)))}");
        blueprint.cellScale = cellScale;
        FpsProcAreaData d = new FpsProcAreaData(){origin=new Vector3(10,0,10), gridSize=gridSize, cellScale=cellScale, tilemap=blueprint.tilemap};
        FindObjectOfType<ProcFpsConstructor>().InstantiateTilemap(d);
        // FindObjectOfType<ProcFpsGeneratorBuilding>().CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }


}
