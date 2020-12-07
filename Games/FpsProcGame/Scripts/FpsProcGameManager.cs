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

    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        GameObject go = new GameObject();
        FpsProcBldgOffice officeBldg = FindObjectOfType<FpsProcBldgOffice>();
        officeBldg.Generate(new FpsProcAreaGenInput(){gridSize=gridSize, npcAmount=npcAmount});
        Debug.Log($"something: {officeBldg.data.TilemapToStr()}");
        officeBldg.data.origin = new Vector3(10,0,10);
        officeBldg.data.cellScale = cellScale;
        officeBldg.Instantiate(pfNpc);
        EnterBuilding(officeBldg);
        StartCoroutine(CrMission());
        // FindObjectOfType<ProcFpsGeneratorBuilding>().CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }

    public void EnterBuilding(FpsProcBldg bldg){
        textAreaName.text = bldg.data.name;
        textAreaMap.text = bldg.data.TilemapToStr();
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
