using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SrpgController : MonoBehaviour {
    
    protected SrpgAudioSource audioSource;
    [SerializeField] protected TMP_Text teamText;
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
        InitializeTeams();
        // semaphor = new ActiveSemaphor();
        // semaphor.objects.Add(fieldCursor.gameObject);
        // semaphor.objects.Add(unitMenu.gameObject);
    }

    private void InitializeTeams(){
        SrpgUnit[] units = FindObjectsOfType<SrpgUnit>();
        unitsByTeam = units.ToLookup(unit => unit.teamId);
        unitsByPosition = units.ToLookup(unit => unit.gameObject.transform.position);
        teamIds = unitsByTeam.Select(g => g.Key).ToList();
        units.ToList().ForEach(unit => InitializeUnit(unit));
        teamText.text = $"{curTeam}'s Turn";
    }

    private void InitializeUnit(SrpgUnit unit){
        if(unit.teamId == curTeam){
            unit.ToIdle();
            unit.hasAttackedThisTurn = false;
        } else {
            unit.state = SrpgUnit.State.Spent; // Using "unit.ToSpent()" here results in unexpected early call to CheckForTurnChange.
        }
    }

    public void CheckForTurnChange(){
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

    public virtual void ChangeTurn(){
        curTeam = curTeam == "good guys" ? "bad guys" : "good guys";
        InitializeTeams();
        Debug.Log($"{curTeam}'s Turn.");
    }

    public void ToggleFieldCursor(bool activate){
        fieldCursor.gameObject.SetActive(activate);
    }

    private void UpdateUnitColliders(){
        unitColls = FindObjectsOfType<SrpgUnit>().Select(unit => unit.GetComponent<BoxCollider2D>()).ToList();
    }

    public List<BoxCollider2D> GetUnitColliders() { return unitColls; }
}