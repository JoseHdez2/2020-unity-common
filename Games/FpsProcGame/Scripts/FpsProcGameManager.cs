using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
using TMPro;

public class FpsProcGameManager : MonoBehaviour
{
    [SerializeField] TMP_Text textAreaName, textTarget, textAreaMap;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;
    [SerializeField] int npcAmount;
    [SerializeField] FpsProcNpc pfNpc;

    List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    List<FpsProcNpc> items = new List<FpsProcNpc>();
    public FpsProcNpc targetNpc;

    void Start()
    {
        FpsProcAreaData area = FindObjectOfType<FpsProcAreaBuilding>().GenerateArea(gridSize);
        Debug.Log($"something: {area.TilemapToStr()}");
        area.origin = new Vector3(10,0,10);
        area.gridSize = gridSize;
        area.cellScale = cellScale;
        EnterArea(area);
        StartCoroutine(CrMission());
        // FindObjectOfType<ProcFpsGeneratorBuilding>().CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }

    public void EnterArea(FpsProcAreaData area){
        textAreaName.text = area.name;
        textAreaMap.text = area.TilemapToStr();
        FindObjectOfType<ProcFpsConstructor>().InstantiateTilemap(area);
        Bounds areaBounds = area.GetBounds();
        var npcsParent = new GameObject("npcs");
        foreach(int i in Enumerable.Range(0, npcAmount)){
            npcs.Add(Instantiate(pfNpc, areaBounds.RandomPos(), Quaternion.identity, npcsParent.transform));
        }
    }

    public void EnterFloor(FpsProcAreaData area, int floorNum){
        textAreaName.text = $"{area.name} F{floorNum}";
        
    }

    private IEnumerator CrMission()
    {
        targetNpc = npcs.RandomItem();
        yield return new WaitForSeconds(1f);
        textTarget.text = $"Target: {targetNpc.data.fullName}";
    }

    public void ClickNpc(FpsProcNpc clickedNpc){
        Debug.Log($"You clicked on {clickedNpc.data.fullName}.");
    }


}
