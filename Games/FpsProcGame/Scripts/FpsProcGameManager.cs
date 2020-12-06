using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class FpsProcGameManager : MonoBehaviour
{
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;
    [SerializeField] int npcAmount;
    [SerializeField] FpsProcNpc pfNpc;

    List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    List<FpsProcNpc> items = new List<FpsProcNpc>();
    public FpsProcNpc targetNpc;

    void Start()
    {
        FpsProcAreaData blueprint = FindObjectOfType<FpsProcAreaBuilding>().GenerateArea(gridSize);
        Debug.Log($"something: {string.Join("\n\n", blueprint.tilemap.Select(f => string.Join("\n", f)))}");
        blueprint.cellScale = cellScale;
        FpsProcAreaData d = new FpsProcAreaData(){origin=new Vector3(10,0,10), gridSize=gridSize, cellScale=cellScale, tilemap=blueprint.tilemap};
        FindObjectOfType<ProcFpsConstructor>().InstantiateTilemap(d);
        Bounds areaBounds = d.GetBounds();
        var npcsParent = new GameObject("npcs");
        foreach(int i in Enumerable.Range(0, npcAmount)){
            npcs.Add(Instantiate(pfNpc, areaBounds.RandomPos(), Quaternion.identity, npcsParent.transform));
        }
        targetNpc = npcs.RandomItem();
        Debug.Log($"Your target is {targetNpc.data.fullName}. You have 60 seconds.");
        // FindObjectOfType<ProcFpsGeneratorBuilding>().CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }

    public void ClickNpc(FpsProcNpc clickedNpc){
        
    }


}
