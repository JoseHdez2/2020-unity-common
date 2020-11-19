using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;

public class SrpgController : MonoBehaviour {
    
    protected SrpgAudioSource audioSource;
    [SerializeField] protected TMP_Text teamText; // TODO this is UI and should probably go in SrpgEmblemController.
    // Teams and turns
    protected List<string> teamIds;
    protected string curTeam = "good guys";

    protected ILookup<string, SrpgUnit> unitsByTeam;
    protected ILookup<Vector3, SrpgUnit> unitsByPosition; // TODO
    private List<BoxCollider2D> unitColls;

    public ActiveSemaphor semaphor = new ActiveSemaphor(); // TODO
    protected SrpgFieldCursor fieldCursor;
    protected SrpgEnemyCursor enemyCursor;
    protected SrpgUnitMenu unitMenu;
    
    [SerializeField] private TextAsset settingsJsonFile;
    public SrpgSettings settings;

    protected void Start() {
        audioSource = FindObjectOfType<SrpgAudioSource>();
        fieldCursor = FindObjectOfType<SrpgFieldCursor>();
        enemyCursor = FindObjectOfType<SrpgEnemyCursor>();
        unitMenu = FindObjectOfType<SrpgUnitMenu>();
        settings = JsonUtility.FromJson<SrpgSettings>(settingsJsonFile.text);

        enemyCursor.gameObject.SetActive(false);
        ChangeTurn(firstTurn: true, forceTeamId: "good guys");
        // semaphor = new ActiveSemaphor();
        // semaphor.objects.Add(fieldCursor.gameObject);
        // semaphor.objects.Add(unitMenu.gameObject);
    }

    public void ChangeTurn(bool firstTurn = false, string forceTeamId = null){
        ToggleFieldCursor(false);
        curTeam = curTeam == "good guys" ? "bad guys" : "good guys";
        if(forceTeamId != null){
            curTeam = forceTeamId;
        }
        StartTurn(curTeam);
    }

    private void StartTurn(string teamId){
        UpdateTeamsSoft();
        SrpgUnit[] units = FindObjectsOfType<SrpgUnit>();
        units.ToList().ForEach(unit => InitializeUnit(unit, teamId));
        teamText.text = $"{teamId}'s Turn";
        Debug.LogFormat($"<color=green>{teamId}'s Turn.</color>");
        StartTurnChild();
    }

    protected virtual void StartTurnChild(){
        ToggleFieldCursor(true);
    }

    public void ToggleFieldCursorFalse(){
        ToggleFieldCursor(false);
    }

    protected void ToggleFieldCursor(bool activate){
        if(curTeam == "good guys"){
            if(activate){
                fieldCursor.gameObject.SetActive(true);
            } else {
                fieldCursor.SelfDisable();
            }
        } else {
            if(activate){
                enemyCursor.gameObject.SetActive(true);
                enemyCursor.StartTurn();
            } else {
                enemyCursor.SelfDisable();
            }
        }
    }

    // Note: 'hard' means this method also checks for turn change / game end.
    public void UpdateTeamsHard(){
        UpdateTeamsSoft();
        CheckForTurnChangeOrGameEnd();
    }

    public void UpdateTeamsSoft(){
        SrpgUnit[] units = FindObjectsOfType<SrpgUnit>();
        UpdateUnitColliders(units);
        unitsByTeam = units.ToLookup(unit => unit.teamId);
        unitsByPosition = units.ToLookup(unit => unit.gameObject.transform.position);
        teamIds = unitsByTeam.Select(g => g.Key).ToList();
    }

    private void CheckForTurnChangeOrGameEnd(){
        if(HasGameEnded()){
            EndGame();
        } else {
            int unitsNotSpent = unitsByTeam[curTeam].Count(unit => unit.state != SrpgUnit.State.Spent);
            if(unitsNotSpent > 0){
                Debug.LogFormat($"<color=gray>{unitsNotSpent} unit(s) remain.</color>");
                ToggleFieldCursor(true); // TODO show cursor after a unit is spent. is this the best place?
                return;
            } else {
                ChangeTurn();
                SrpgUnit[] units = FindObjectsOfType<SrpgUnit>();
                unitsByPosition = units.ToLookup(unit => unit.gameObject.transform.position);
            }
        }
    }
    
    private bool HasGameEnded(){
        return teamIds.Count() == 1 || teamIds.Where(t => IsTeamAlive(t)).Count() == 1;
    }

    protected bool IsTeamAlive(string teamId){
        return unitsByTeam[teamId] != null && unitsByTeam[teamId].Any(u => u.IsAlive());
    }

    protected virtual void EndGame(){
        Debug.Log("Game has ended!");
    }

    private void InitializeUnit(SrpgUnit unit, string teamId){
        if(unit.teamId == teamId){
            unit.ToIdle();
            unit.hasAttackedThisTurn = false;
        } else {
            unit.ToSpent(hard: false);
        }
    }

    private void UpdateUnitColliders(SrpgUnit[] units){
        unitColls = units.Where(u => u.IsAlive()).Select(unit => unit.GetComponent<BoxCollider2D>()).ToList();
    }

    public List<BoxCollider2D> GetUnitColliders() { return unitColls; }
}