using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class FpsProcGoal {
    public enum Type {Sabotage, Contact, Extract, Neutralize}
    public Type type;
    public GameObject target;
    public string targetName;
    public string ToStr() => $"{type} {targetName}";
}

public class FpsProcGameManager : MonoBehaviour
{
    List<FpsProcGoal> goals = new List<FpsProcGoal>();
    [SerializeField] TMP_Text textAreaName, textTarget, textAreaMap, textBriefing;
    [SerializeField] AnimFade briefingImage;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;
    [SerializeField] int npcAmount;
    [SerializeField] public FpsProcNpc pfNpc;

    List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    List<FpsProcNpc> items = new List<FpsProcNpc>();
    public FpsProcNpcData targetNpc;

    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        StartCoroutine(CrMission());
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.B)){
            StartCoroutine(ShowBriefing());
        }
    }

    private IEnumerator ShowBriefing(){
        briefingImage.Toggle(true);
        yield return new WaitForSeconds(5f);
        briefingImage.Toggle(false);
    }    

    public void EnterBuilding(FpsProcBldg bldg){
        textAreaName.text = bldg.data.name;
        textAreaMap.text = bldg.data.TilemapToStr();
    }

    public void EnterFloor(FpsProcBldgData area, int floorNum){
        textAreaName.text = $"{area.name} F{floorNum}";
    }

    private IEnumerator CrMission(){
        yield return new WaitForSeconds(1f);
        targetNpc = FindObjectsOfType<FpsProcBldg>().SelectMany(bldg => bldg.npcsData).ToList().RandomItem();
        textTarget.text = $"Target: {targetNpc.fullName}";
    }

    public void ClickNpc(FpsProcNpc clickedNpc){
        Debug.Log($"You clicked on {clickedNpc.data.fullName}.");
    }


}
