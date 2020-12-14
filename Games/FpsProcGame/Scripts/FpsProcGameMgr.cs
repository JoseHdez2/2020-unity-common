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
    public enum Type {Interrogate, Contact, Extract, Kill, Investigate}
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
    [SerializeField] ImageWipe bgConversation;
    [SerializeField] ButtonMenu notebookButtons;
    [SerializeField] VfxLerpInOut notebook;
    [SerializeField] AnimFade conversationCanvasGroup;


    [Header("Data Stuff")]
    [SerializeField] public FpsProcNpc pfNpc;
    public List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    
    public List<FpsProcNpcData> npcsData;
    [SerializeField] int npcsAmount;

    internal void RepositionNpcs()
    {
        foreach(FpsProcNpc npc in npcs){
            List<FpsProcOrganization> orgs = affiliationsMgr.GetOrganizations(npc.data.uuid);
            FpsProcBounds fpsProcBounds;
            if(orgs.IsEmpty()) {
                Debug.LogError("Npc has no organization!");
                fpsProcBounds = affiliationsMgr.organizations.RandomItem().areas.RandomItem();
            } else {
                Debug.Log(orgs.Count);
                fpsProcBounds = orgs.RandomItem().areas.RandomItem();
            }
            npc.transform.position = fpsProcBounds.RandomNpcSpawnPos();
        }
    }

    List<FpsProcNpc> items = new List<FpsProcNpc>();
    List<FpsProcGoal> goals = new List<FpsProcGoal>();
    [NonSerialized] public FpsProcNpc targetNpc, npcWeAreTalkingWith;
    private MouseLook mouseLook;
    private PlayerMovement playerController;
    [NonSerialized] public FpsProcDatabase database;
    [NonSerialized] public FpsProcNpcRelationMgr relationsMgr;
    [NonSerialized] public FpsProcNpcAffiliationMgr affiliationsMgr;


    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        mouseLook = FindObjectOfType<MouseLook>();
        playerController = FindObjectOfType<PlayerMovement>();
        database = FindObjectOfType<FpsProcDatabase>();
        relationsMgr = FindObjectOfType<FpsProcNpcRelationMgr>();
        affiliationsMgr = FindObjectOfType<FpsProcNpcAffiliationMgr>();
        TogglePlayerControls(false);
        database.Initialize();
        StartMission();
        FindObjectsOfType<FpsProcDistrict>().ToList().ForEach(d => d.GenerateAndInstantiateBuildings());
        TogglePlayerControls(true);
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
        conversationCanvasGroup.Toggle(enable);
        bgConversation.ToggleWipe(enable);
        notebook.Toggle(enable);
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

    private void StartMission(){
        npcs = InstantiateNpcs();
        npcsData = npcs.Select(n => n.data).ToList();
        // relationsMgr.relations = relationsMgr.GenerateRelationships(npcs, finalNpc: targetNpc);
        affiliationsMgr.organizations = affiliationsMgr.GenerateOrganizations();
        affiliationsMgr.organizations = affiliationsMgr.GenerateAffiliations(affiliationsMgr.organizations, npcs);
        string targetUuid = affiliationsMgr.organizations.RandomItem().members.Keys.ToList().RandomItem();
        targetNpc = npcs.FirstOrDefault(n => n.data.uuid == targetUuid);
        goals.Add(new FpsProcGoal(){type=FpsProcGoal.Type.Kill, target=targetNpc.gameObject, targetName=targetNpc.data.fullName});
        FpsProcOrganization targetOrg = affiliationsMgr.GetOrganizations(targetNpc.data.uuid).RandomItem();
        goals.Add(new FpsProcGoal(){type=FpsProcGoal.Type.Investigate, targetName=targetOrg.name});
        UpdateNotepad();
    }

    private List<FpsProcNpc> InstantiateNpcs(){
        var npcsParent = new GameObject("npcs");
        npcsParent.transform.parent = transform;
        List<FpsProcNpc> newNpcs = new List<FpsProcNpc>();
        foreach(int i in Enumerable.Range(0, npcsAmount)){
            FpsProcNpc npc = Instantiate(pfNpc, new Vector3(), Quaternion.identity, npcsParent.transform);
            npc.data = new FpsProcNpcData(database);
            newNpcs.Add(npc);
        }
        return newNpcs;
    }

    public void ClickNpc(FpsProcNpc clickedNpc, string greeting = "Hello."){
        if(npcWeAreTalkingWith == clickedNpc) return;
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

    public void AskNpcName(){
        Say($"My name is {npcWeAreTalkingWith.data.fullName}.");
        npcWeAreTalkingWith.ToggleName(true);
    }

    public void AskNpcJob(){
        List<FpsProcOrganization> orgs = affiliationsMgr.GetOrganizations(npcWeAreTalkingWith.data.uuid);
        if(orgs.IsEmpty()){
            Say($"I am currently unemployed.");
        } else {
            int rank = affiliationsMgr.GetAffiliationRank(npcWeAreTalkingWith.data.uuid, orgs[0].name);
            string rankName = orgs[0].GetRankName(rank);
            if(rank == 0){
                Say($"I'm the {rankName} of the {orgs[0].name}.");
            } else {
                Say($"I'm {rankName.A_An()} {rankName} in the {orgs[0].name}.");
            }
        }
        npcWeAreTalkingWith.ToggleJob(true);
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
