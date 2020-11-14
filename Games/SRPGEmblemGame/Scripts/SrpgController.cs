using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    protected SrpgUnitMenu unitMenu;

    protected void Start() {
        audioSource = FindObjectOfType<SrpgAudioSource>();
        fieldCursor = FindObjectOfType<SrpgFieldCursor>();
        unitMenu = FindObjectOfType<SrpgUnitMenu>();
        UpdateUnitColliders();
        ChangeTurn(firstTurn: true, forceTeamId: "good guys");
        // semaphor = new ActiveSemaphor();
        // semaphor.objects.Add(fieldCursor.gameObject);
        // semaphor.objects.Add(unitMenu.gameObject);
    }

    public virtual void ChangeTurn(bool firstTurn = false, string forceTeamId = null){
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
        Debug.Log($"{teamId}'s Turn.");
    }

    public void ToggleFieldCursor(bool activate){
        fieldCursor.gameObject.SetActive(activate);
    }

    // Note: this method also checks for turn change / game end.
    public void UpdateTeamsHard(){
        UpdateTeamsSoft();
        CheckForTurnChangeOrGameEnd();
    }

    private void UpdateTeamsSoft(){
        SrpgUnit[] units = FindObjectsOfType<SrpgUnit>();
        unitsByTeam = units.ToLookup(unit => unit.teamId);
        unitsByPosition = units.ToLookup(unit => unit.gameObject.transform.position);
        teamIds = unitsByTeam.Select(g => g.Key).ToList();
    }

    private void CheckForTurnChangeOrGameEnd(){
        if(teamIds.Count == 1){
            EndGame();
        } else {
            int unitsNotSpent = unitsByTeam[curTeam].Count(unit => unit.state != SrpgUnit.State.Spent);
            if(unitsNotSpent > 0){
                Debug.Log($"{unitsNotSpent} unit(s) remain.");
                return;
            } else {
                ChangeTurn();
                SrpgUnit[] units = FindObjectsOfType<SrpgUnit>();
                unitsByPosition = units.ToLookup(unit => unit.gameObject.transform.position);
            }
        }
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

    private void UpdateUnitColliders(){
        unitColls = FindObjectsOfType<SrpgUnit>().Select(unit => unit.GetComponent<BoxCollider2D>()).ToList();
    }

    public List<BoxCollider2D> GetUnitColliders() { return unitColls; }
}