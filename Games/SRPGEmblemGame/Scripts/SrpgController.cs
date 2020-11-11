using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SrpgController : MonoBehaviour {
    
    protected SrpgAudioSource audioSource;
    [SerializeField] protected TMP_Text teamText;
    // Teams and turns
    protected ILookup<string, SRPGUnit> unitsByTeam;
    protected List<string> teamIds;
    protected string curTeam = "good guys";

    public ActiveSemaphor semaphor = new ActiveSemaphor();
    protected SrpgFieldCursor fieldCursor;
    protected SRPGUnitMenu unitMenu;

    protected void Start() {
        audioSource = FindObjectOfType<SrpgAudioSource>();
        fieldCursor = FindObjectOfType<SrpgFieldCursor>();
        unitMenu = FindObjectOfType<SRPGUnitMenu>();
        InitializeTeams();
        // semaphor = new ActiveSemaphor();
        // semaphor.objects.Add(fieldCursor.gameObject);
        // semaphor.objects.Add(unitMenu.gameObject);
    }

    private void InitializeTeams(){
        SRPGUnit[] units = FindObjectsOfType<SRPGUnit>();
        unitsByTeam = units.ToLookup(unit => unit.teamId);
        teamIds = unitsByTeam.Select(g => g.Key).ToList();
        units.ToList().ForEach(unit => InitializeUnit(unit));
        teamText.text = $"{curTeam}'s Turn";
    }

    private void InitializeUnit(SRPGUnit unit){
        if(unit.teamId == curTeam){
            unit.ToIdle();
        } else {
            unit.state = SRPGUnit.State.Spent; // Using "unit.ToSpent()" here results in unexpected early call to CheckForTurnChange.
        }
    }

    public void CheckForTurnChange(){
        int unitsNotSpent = unitsByTeam[curTeam].Count(unit => unit.state != SRPGUnit.State.Spent);
        if(unitsNotSpent > 0){
            Debug.Log($"{unitsNotSpent} unit(s) remain.");
            return;
        } else {
            ChangeTurn();
        }
    }

    public virtual void ChangeTurn(){
        curTeam = curTeam == "good guys" ? "bad guys" : "good guys";
        InitializeTeams();
        Debug.Log($"{curTeam}'s Turn.");
    }

    public void ToggleFieldCursor(bool activate){
        Debug.Log($"ToggleFieldCursor:{activate}");
        fieldCursor.gameObject.SetActive(activate);
    }
}