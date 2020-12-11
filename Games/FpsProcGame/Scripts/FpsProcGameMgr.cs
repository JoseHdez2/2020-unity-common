using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FpsProcGoal {
    public enum Type {Interrogate, Contact, Extract, Neutralize}
    public Type type;
    public GameObject target;
    public string targetName;
    public string ToStr() => $"{type} {targetName}";
}

public class FpsProcGameMgr : MonoBehaviour
{
    List<FpsProcGoal> goals = new List<FpsProcGoal>();
    [SerializeField] TMP_Text textAreaName, textTarget, textAreaMap, textNotepad, textConversation;
    [SerializeField] Button btnGoodbye;
    [SerializeField] AnimFade missionImage;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;
    [SerializeField] int npcAmount;
    [SerializeField] public FpsProcNpc pfNpc;

    public List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    List<FpsProcNpc> items = new List<FpsProcNpc>();
    public FpsProcNpc targetNpc;
    private MouseLook mouseLook;
    private PlayerMovement playerController;

    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        mouseLook = FindObjectOfType<MouseLook>();
        playerController = FindObjectOfType<PlayerMovement>();
        TogglePlayerControls(false);
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
        TogglePlayerControls(!enable);
    }

    public void TogglePlayerControls(bool enable){
        mouseLook.enabled = enable;
        playerController.enabled = enable;
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

    string[] intro = {"In the future", "there is no privacy.", "People live in fear", "and secret agents uphold the order.", "they are called", "THE BLADERUNNERS"};
    string[] intro2 = {"In the future", "corporations have taken over.", "A.I. reigns supreme", "and secret agents do its bidding.", "they are called", "THE BLADERUNNERS"};
    string[] intro3 = {"In the future", "life has no value.", "In a certain page, for the right price", "secret agents will kill for you.", "they are called", "THE BLADERUNNERS"};

    private IEnumerator CrMission(){
        yield return new WaitForSeconds(1f);
        npcs = FindObjectsOfType<FpsProcBldg>().SelectMany(bldg => bldg.npcs).ToList();
        targetNpc = npcs.RandomItem();
        FpsProcGoal goal = new FpsProcGoal(){type=FpsProcGoal.Type.Neutralize, target=targetNpc.gameObject, targetName=targetNpc.data.fullName};
        goals.Add(goal);
        textTarget.text = $"<u>M</u>ission: {goal.ToStr()}";
        TogglePlayerControls(true);
    }

    public void ClickNpc(FpsProcNpc clickedNpc, string greeting = "Hello."){
        Debug.Log($"You clicked on {clickedNpc.data.fullName}.");
        textConversation.text = greeting;
        ToggleConversation(true);
    }


}
