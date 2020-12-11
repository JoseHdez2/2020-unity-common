using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class FpsProcGoal {
    public enum Type {Interrogate, Contact, Extract, Neutralize, Investigate}
    public Type type;
    public GameObject target;
    public string targetName;
    public bool completed;
    public string ToStrPro() => $"{(completed ? $"<s>{ToStr()}</s>" : ToStr())}";
    public string ToStr() => $"{type} <u>{targetName}</u>.";
}

public class FpsProcGameMgr : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField] Button btnGoodbye;
    [SerializeField] TMP_Text textAreaName, textTarget, textAreaMap, textConversation;
    [SerializeField] ButtonMenu notebookButtons;
    [SerializeField] VfxLerpInOut notebook;

    [Header("Data Stuff")]
    [SerializeField] public FpsProcNpc pfNpc;
    public List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    List<FpsProcNpc> items = new List<FpsProcNpc>();
    List<FpsProcGoal> goals = new List<FpsProcGoal>();
    public FpsProcNpc targetNpc, npcWeAreTalkingWith;
    private MouseLook mouseLook;
    private PlayerMovement playerController;
    private FpsProcRelationMgr relationsMgr;


    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        mouseLook = FindObjectOfType<MouseLook>();
        playerController = FindObjectOfType<PlayerMovement>();
        relationsMgr = FindObjectOfType<FpsProcRelationMgr>();
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

    public void EndConversation() => ToggleConversation(false);

    private void ToggleConversation(bool enable, FpsProcNpc talkingWith = null){
        UpdateNotepad();
        textConversation.enabled = enable;
        notebook.Toggle(enable);
        btnGoodbye.gameObject.SetActive(enable);
        textAreaMap.enabled = !enable;
        textAreaName.enabled = !enable;
        TogglePlayerControls(!enable);
        npcWeAreTalkingWith = (enable && talkingWith) ? talkingWith : null;
    }

    public void TogglePlayerControls(bool enable){
        mouseLook.enabled = enable;
        playerController.enabled = enable;
    }

    private IEnumerator ShowBriefing(){
        notebook.Toggle(true);
        yield return new WaitForSeconds(5f);
        notebook.Toggle(false); // TODO what if player starts a conversation before 5s pass?
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
        UpdateNotepad();
        relationsMgr.relations = relationsMgr.GenerateRelationships(npcs, finalNpc: targetNpc);
        TogglePlayerControls(true);
    }

    public void ClickNpc(FpsProcNpc clickedNpc, string greeting = "Hello."){
        Debug.Log($"You clicked on {clickedNpc.data.fullName}.");
        textConversation.text = greeting;
        ToggleConversation(true, clickedNpc);
    }

    public void InterrogateNpc(FpsProcNpc npc, FpsProcGoal subject){
        FpsProcRelationship relationship = relationsMgr.GetRelationship(npc.data.fullName, subject.targetName);
        if(relationship != null){
            FpsProcNpc targetNpc = npcs.FirstOrDefault(n => n == subject.target);
            string place = "{targetNpc.data.bldgName}, {targetNpc.data.bldgFloor}";
            Say($"Oh, I know {subject.targetName}. They're a(n) {relationship.type}. They're in {place}.");
            goals.Add(new FpsProcGoal(){type=FpsProcGoal.Type.Investigate, targetName=place});
            UpdateNotepad();
        } else {
            Say("Sorry, I don't know about that.");
        }
    }

    private void UpdateNotepad(){
        notebookButtons.buttonsData = goals.Select(g => new ButtonData(){name=g.ToStrPro(), interactable=!g.completed, action = new UnityEvent()}).ToList();
        notebookButtons.RefreshButtons();
        // TODO play writing on paper sound.
    }

    private void Say(string str){
        textConversation.text = str;
    }
}
