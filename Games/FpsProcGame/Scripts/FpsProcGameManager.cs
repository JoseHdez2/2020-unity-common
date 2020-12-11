using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] TMP_Text textAreaName, textTarget, textAreaMap, textNotepad, textConversation;
    [SerializeField] Button btnGoodbye;
    [SerializeField] AnimFade missionImage;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;
    [SerializeField] int npcAmount;
    [SerializeField] public FpsProcNpc pfNpc;

    List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    List<FpsProcNpc> items = new List<FpsProcNpc>();
    public FpsProcNpcData targetNpc;
    private MouseLook mouseLook;
    private PlayerMovement playerController;

    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        mouseLook = FindObjectOfType<MouseLook>();
        playerController = FindObjectOfType<PlayerMovement>();
        StartCoroutine(CrMission());
        ToggleConversation(false);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.B)){
            StartCoroutine(ShowBriefing());
        }
        if(Input.GetButton("Cancel") && !mouseLook.enabled){
            ToggleConversation(false);
        }
    }

    public void ToggleConversation(bool enable){
        textConversation.enabled = enable;
        textNotepad.enabled = enable;
        btnGoodbye.gameObject.SetActive(enable);
        textAreaMap.enabled = !enable;
        textAreaName.enabled = !enable;
        mouseLook.enabled = !enable;
        playerController.enabled = !enable;
    }

    private IEnumerator ShowBriefing(){
        missionImage.Toggle(true);
        yield return new WaitForSeconds(5f);
        missionImage.Toggle(false);
    }    

    public void EnterFloor(FpsProcBldgData area, int floorNum){
        textAreaName.text = $"{area.name} F{floorNum}";
        textAreaMap.text = area.TilemapToStr(floorNum);
    }
    
    public void ExitFloor(){
        textAreaName.text = "";
        textAreaMap.text = "";
    }

    private IEnumerator CrMission(){
        yield return new WaitForSeconds(1f);
        targetNpc = FindObjectsOfType<FpsProcBldg>().SelectMany(bldg => bldg.npcsData).ToList().RandomItem();
        textTarget.text = $"Target: {targetNpc.fullName}";
    }

    public void ClickNpc(FpsProcNpc clickedNpc, string greeting = "Hello."){
        Debug.Log($"You clicked on {clickedNpc.data.fullName}.");
        textConversation.text = greeting;
        ToggleConversation(true);
    }


}
