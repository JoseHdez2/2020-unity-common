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
    public int depth = 0;
    public string ToStrPro() => $"{(depth > 0 ? new string(' ', depth) + "⮤ " : "")}{(completed ? $"<s>{ToStr()}</s>" : ToStr())}";
    public string ToStr() => $"{type} <u>{targetName}</u>.";
}

public class FpsProcGameMgr : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField] Button btnGoodbye;
    [SerializeField] TMP_Text textAreaName, textAreaMap, textConversation;
    [SerializeField] ImageWipe bgConversation, bgGoodbye;
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
    private FpsProcNpcRelationMgr relationsMgr;
    private FpsProcNpcAffiliationMgr affiliationsMgr;


    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        mouseLook = FindObjectOfType<MouseLook>();
        playerController = FindObjectOfType<PlayerMovement>();
        relationsMgr = FindObjectOfType<FpsProcNpcRelationMgr>();
        affiliationsMgr = FindObjectOfType<FpsProcNpcAffiliationMgr>();
        TogglePlayerControls(false);
        StartCoroutine(CrMission());
        ToggleConversation(false);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)){
            StartCoroutine(ShowNotepad());
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

    private IEnumerator ShowNotepad(){
        notebook.Toggle(true);
        yield return new WaitForSeconds(5f);
        notebook.Toggle(false); // TODO what if player starts a conversation before 5s pass?
    }    

    public void EnterFloor(FpsProcBounds floor){
        textAreaName.text = floor.ToStr();
        textAreaMap.text = floor.bldg.data.TilemapToStr(floor.floorNum);
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
        relationsMgr.relations = relationsMgr.GenerateRelationships(npcs, finalNpc: targetNpc);
        affiliationsMgr.organizations = affiliationsMgr.GenerateOrganizations();
        affiliationsMgr.organizations = affiliationsMgr.GenerateAffiliations(affiliationsMgr.organizations, npcs);
        Debug.Log(affiliationsMgr.organizations.Count);
        goals.Add(new FpsProcGoal(){type=FpsProcGoal.Type.Neutralize, target=targetNpc.gameObject, targetName=targetNpc.data.fullName});
        Debug.Log($"Organizations for targetNpc: {affiliationsMgr.GetOrganizations(targetNpc.data.fullName).Count}");
        FpsProcOrganization targetOrg = affiliationsMgr.GetOrganizations(targetNpc.data.uuid).RandomItem();
        goals.Add(new FpsProcGoal(){type=FpsProcGoal.Type.Investigate, targetName=targetOrg.name});
        UpdateNotepad();
        TogglePlayerControls(true);
    }

    public void ClickNpc(FpsProcNpc clickedNpc, string greeting = "Hello."){
        Debug.Log($"You clicked on {clickedNpc.data.uuid}.");
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
