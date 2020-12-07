using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class FpsProcGameManager : MonoBehaviour
{
    [SerializeField] TMP_Text textAreaName, textTarget, textAreaMap;
    [SerializeField] Image briefingImage;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;
    [SerializeField] int npcAmount;
    [SerializeField] public FpsProcNpc pfNpc;

    List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    List<FpsProcNpc> items = new List<FpsProcNpc>();
    public FpsProcNpc targetNpc;

    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        // StartCoroutine(CrMission());
        // FindObjectOfType<ProcFpsGeneratorBuilding>().CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }

    public void EnterBuilding(FpsProcBldg bldg){
        textAreaName.text = bldg.data.name;
        textAreaMap.text = bldg.data.TilemapToStr();
    }

    public void EnterFloor(FpsProcBldgData area, int floorNum){
        textAreaName.text = $"{area.name} F{floorNum}";
    }

    private IEnumerator CrMission(){
        targetNpc = npcs.RandomItem();
        yield return new WaitForSeconds(1f);
        textTarget.text = $"Target: {targetNpc.data.fullName}";
    }

    public void ClickNpc(FpsProcNpc clickedNpc){
        Debug.Log($"You clicked on {clickedNpc.data.fullName}.");
    }


}
