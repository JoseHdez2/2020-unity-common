using System;
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
        Debug.Log($"something: {blueprint.TilemapToStr()}");
        blueprint.origin = new Vector3(10,0,10);
        blueprint.gridSize = gridSize;
        blueprint.cellScale = cellScale;
        FindObjectOfType<ProcFpsConstructor>().InstantiateTilemap(blueprint);
        Bounds areaBounds = blueprint.GetBounds();
        var npcsParent = new GameObject("npcs");
        foreach(int i in Enumerable.Range(0, npcAmount)){
            npcs.Add(Instantiate(pfNpc, areaBounds.RandomPos(), Quaternion.identity, npcsParent.transform));
        }
        StartCoroutine(CrMission());
        // FindObjectOfType<ProcFpsGeneratorBuilding>().CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }

    private IEnumerator CrMission()
    {
        targetNpc = npcs.RandomItem();
        yield return new WaitForSeconds(1f);
        Debug.Log($"Your target is {targetNpc.data.fullName}. You have 60 seconds.");
    }

    public void ClickNpc(FpsProcNpc clickedNpc){
        
    }


}
