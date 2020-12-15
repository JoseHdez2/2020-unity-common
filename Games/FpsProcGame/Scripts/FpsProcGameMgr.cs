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
    public string ToStrPro2() => $"{(depth > 0 ? new string(' ', depth) + " " : "")}{(completed ? $"<s>{targetName}</s>" : targetName)}"; // QnD
    public string ToStr() => $"{type} <u>{targetName}</u>.";
}

public class FpsProcGameMgr : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField] Button btnGoodbye;
    [SerializeField] TMP_Text textAreaName, textAreaMap, textConversation, textTimer;
    [SerializeField] AudioSource playerAudioSource, creditsMusic;
    [SerializeField] AudioClip soundCock, soundShot, soundWrite;
    [SerializeField] ImageWipe bgConversation;
    [SerializeField] ButtonMenu notebookButtons;
    [SerializeField] VfxLerpInOut notebook, gun, titleLerp;
    [SerializeField] AnimFade conversationCanvasGroup, screenWipe, creditsFade;
    [SerializeField] CanvasGroup screenWipe2, titleCanvasGroup;


    [Header("Data Stuff")]
    [SerializeField] public FpsProcNpc pfNpc;
    public List<FpsProcNpc> npcs = new List<FpsProcNpc>();
    
    public List<FpsProcNpcData> npcsData;
    [SerializeField] int npcsAmount;
    private OneTimeTimer missionTimer;
    [SerializeField] private int missionSecs;
    [Range(5,95)]
    [SerializeField] private int chanceNpcHasClue;

    public void RepositionNpcs() {
        foreach(FpsProcNpc npc in npcs){
            List<FpsProcOrganization> orgs = affiliationsMgr.GetOrganizations(npc.data.uuid);
            FpsProcBounds fpsProcBounds;
            if(orgs.IsEmpty()) {
                Debug.LogError("Npc has no organization!");
                fpsProcBounds = FindObjectsOfType<FpsProcBounds>().ToList().RandomItem();
            } else {
                Debug.Log($"# of orgs (should be 1): {orgs.Count}");
                FpsProcOrganization org2 = orgs.FirstOrDefault(org => !org.areas.IsEmpty());
                if(org2 != null){
                    fpsProcBounds = org2.areas.RandomItem();
                } else {
                    fpsProcBounds = FindObjectsOfType<FpsProcBounds>().ToList().RandomItem();
                }
            }
            fpsProcBounds.SetNpcToBounds(npc);
        }
    }

    List<FpsProcNpc> items = new List<FpsProcNpc>();
    List<FpsProcGoal> goals = new List<FpsProcGoal>();
    [NonSerialized] public FpsProcNpc targetNpc, npcWeAreTalkingWith;
    [NonSerialized] private MouseLook mouseLook;
    [NonSerialized] private PlayerMovement playerController;
    [NonSerialized] private FpsProcGameAudioMgr audioMgr;
    [NonSerialized] public FpsProcDatabase database;
    [NonSerialized] public FpsProcNpcRelationMgr relationsMgr;
    [NonSerialized] public FpsProcNpcAffiliationMgr affiliationsMgr;


    // var watch = System.Diagnostics.Stopwatch.StartNew();
    // Debug.Log("${watch.ElapsedTicks}");

    void Start()
    {
        titleLerp.Toggle(true);
        mouseLook = FindObjectOfType<MouseLook>();
        playerController = FindObjectOfType<PlayerMovement>();
        audioMgr = FindObjectOfType<FpsProcGameAudioMgr>();
        database = FindObjectOfType<FpsProcDatabase>();
        relationsMgr = FindObjectOfType<FpsProcNpcRelationMgr>();
        affiliationsMgr = FindObjectOfType<FpsProcNpcAffiliationMgr>();
        TogglePlayerControls(false);
        database.Initialize();
        StartMission();
        missionTimer = new OneTimeTimer(missionSecs);
        audioMgr.StartClockSound();
        FindObjectsOfType<FpsProcDistrict>().ToList().ForEach(d => d.GenerateAndInstantiateBuildings());
        TogglePlayerControls(true);
        ToggleConversation(false);
        AddToNotepad(new FpsProcGoal(){type=FpsProcGoal.Type.Investigate, targetName="Terminate target.", depth=0});
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)){
            StartCoroutine(ShowNotepad());
        }
        if(Input.GetButton("Cancel") && !mouseLook.enabled){
            ToggleConversation(false);
        }
        if(missionTimer.UpdateAndCheck(Time.deltaTime)){
            GameOver();
        }
        textTimer.text = $"Time left: {missionTimer.ToMMSS()}";
    }

    public void EndConversation() => ToggleConversation(false);

    private void ToggleConversation(bool enable, FpsProcNpc talkingWith = null){
        conversationCanvasGroup.Toggle(enable);
        bgConversation.Toggle(enable);
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
        string targetUuid = affiliationsMgr.organizations.RandomItem().GetMembers().RandomItem();
        targetNpc = npcs.FirstOrDefault(n => n.data.uuid == targetUuid);
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

    private enum ClueType {Name, Surname, Job, Org, Bldg, Floor};
    private List<string> dontKnow = new List<string>(){"Sorry, I don't know.", "I don't know about that.", "I don't know what you're talking about, sorry."};
    public void InterrogateNpc(){
        System.Random rand = new System.Random((npcWeAreTalkingWith.data.uuid + goals[0].targetName).GetHashCode()); // always same response for same npc + topic.
        if(rand.Next(0, 100) < chanceNpcHasClue){
            ClueType clueType = RandomUtils.RandomEnumValue<ClueType>(rand);
            AddClue(clueType, rand);
        } else {
            Say(dontKnow.RandomItem(rand));
        }
    }

    private List<string> iKnow = new List<string>(){"Oh, I can help you.", "That person?", "I think I know...", "If you mean them..."};

    private void AddClue(ClueType clueType, System.Random rand)
    {
        string hint = "";
        FpsProcOrganization org = affiliationsMgr.GetOrganizations(targetNpc.data.uuid)[0];
        string rankName = org.GetRankNameForMember(targetNpc.data.uuid);
        FpsProcBounds bounds = FindObjectsOfType<FpsProcBounds>().First(b => b.npcs.Contains(targetNpc));
        switch(clueType){
            case ClueType.Name: hint = $"Their name is {targetNpc.data.fullName.Split(' ')[0]}."; break;
            case ClueType.Surname: hint = $"Their last name is {targetNpc.data.fullName.Split(' ')[1]}."; break;
            case ClueType.Job: hint = $"They're {rankName.A_An()} {rankName}."; break;
            case ClueType.Org: hint = $"They're in the {org.name}."; break;
            case ClueType.Bldg: hint = $"They're in the {bounds.bldg.data.name}."; break;
            case ClueType.Floor: hint = $"They're in the {bounds.floorNum.Ord()} floor of a building."; break;
        };
        Say($"{iKnow.RandomItem(rand)} {hint}");
        AddToNotepad(new FpsProcGoal(){type=FpsProcGoal.Type.Investigate, targetName=hint, depth=1});
    }

    public void AskNpcName(){
        Say($"My name is {npcWeAreTalkingWith.data.fullName}.");
        npcWeAreTalkingWith.ToggleName(true);
    }

    public void AskNpcJob(){
        List<FpsProcOrganization> orgs = affiliationsMgr.GetOrganizations(npcWeAreTalkingWith.data.uuid);
        string rankName = "";
        if(orgs.IsEmpty()){
            Say($"I am currently unemployed.");
            rankName = "Unemployed";
        } else {
            int rank = orgs[0].GetRank(npcWeAreTalkingWith.data.uuid);
            rankName = orgs[0].GetRankName(rank);
            if(rank == 0){
                Say($"I'm the {rankName} of the {orgs[0].name}.");
            } else {
                Say($"I'm {rankName.A_An()} {rankName} in the {orgs[0].name}.");
            }
        }
        npcWeAreTalkingWith.ToggleJob(true, $"{rankName} {(orgs.IsEmpty() ? "" : $"({orgs[0].name})")}");
    }

    private void AddToNotepad(FpsProcGoal goal){
        // UnityEvent unityEvent = new UnityEvent();
        // unityEvent.AddListener(InterrogateNpc);
        // notebookButtons.buttonsData = goals.Select(g => new ButtonData(){name=g.ToStrPro(), interactable=!g.completed, action = unityEvent}).ToList();
        if(goals.Any(g => g.targetName == goal.targetName)) return; // QnD solution to prevent writing the same hint twice.
        goals.Add(goal);
        notebookButtons.buttonsData = goals.Select(g => new ButtonData(){name=g.ToStrPro2(), interactable=false, action= new UnityEvent()}).ToList();
        notebookButtons.RefreshButtons();
        // TODO play writing on paper sound.
    }

    private void Say(string str){
        textConversation.text = str;
    }

    public void KillNpc(){
        StartCoroutine(CrKillNpc());
    }

    public IEnumerator CrKillNpc(){
        bool won = targetNpc == npcWeAreTalkingWith;
        EndGame();
        gun.Toggle(true); // pull up gun model
        playerAudioSource.clip = soundCock; // make cocking sound
        playerAudioSource.Play();
        // screenWipe.Toggle(true); // fade to black
        yield return new WaitForSeconds(2f);
        screenWipe2.alpha = 1f;
        playerAudioSource.clip = soundShot;
        playerAudioSource.Play();// gunshot sound
        yield return new WaitForSeconds(2f); // oblivion
        if(won){
            titleCanvasGroup.alpha = 1f; // show the title!
            creditsMusic.Play();
        }
        yield return new WaitForSeconds(3f); // credits start appearing
        titleLerp.Toggle(false);
        creditsFade.Toggle(true); // roll credits
    }
    private void GameOver()
    {
        TogglePlayerControls(false);
        EndGame();
        screenWipe.Toggle(true);
        creditsFade.Toggle(true); // roll credits
        // slow fade-to-black of shame.
        // show credits.
    }

    private void EndGame() {
        textTimer.text = "";
        screenWipe2.blocksRaycasts = true;
        screenWipe2.interactable = true;
        ToggleConversation(false);
        TogglePlayerControls(false);
        notebook.Toggle(false); // drop notepad
        missionTimer.Reset();
        FindObjectOfType<SelectionManager>().enabled = false;
    }
}
