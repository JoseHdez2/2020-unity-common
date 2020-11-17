using UnityEngine;
using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SrpgEnemyCursor : LerpMovement {
    
    private SrpgAudioSource audioSource;
    private SrpgController srpgController;
    public string teamId;
    public SrpgUnit selectedUnit;

    private void Awake() {
        audioSource = FindObjectOfType<SrpgAudioSource>();
        srpgController = FindObjectOfType<SrpgController>();
    }

    private void OnEnable() {
        StartCoroutine(CrTurn());
    }

    private IEnumerator CrTurn(){
        List<SrpgUnit> units = FindObjectsOfType<SrpgUnit>().ToList().Where(u => u.state == SrpgUnit.State.Idle).ToList();
        while(units.Count > 0) {
            selectedUnit = units[0];
            units.RemoveAt(0);
            StartCoroutine(CrUnitTurn(selectedUnit));
            yield return new WaitUntil(() => selectedUnit == null);
        }
    }

    private IEnumerator CrUnitTurn(SrpgUnit unit){
        MoveAiCursor(unit.transform.position);
        yield return new WaitUntil(() => !destinationPos.HasValue);
        SrpgAttack bestAttack = unit.BestAttack();
        if(bestAttack == null){
            MyExtensions.LogRed("Won't do anything!");
            selectedUnit.ToSpent();
        } else {
            if(!unit.CanAttack(bestAttack)){
                MyExtensions.LogRed("Will move, then attack!");
                Vector2 pos = unit.PositionForAttack(bestAttack);
                MyExtensions.LogRed($"move pos: {pos}");
                unit.Move(pos);
                yield return new WaitUntil(() => selectedUnit.state != SrpgUnit.State.Moving);
            }
            MyExtensions.LogRed("Will attack!");
            unit.Attack(bestAttack.target);
        }
        yield return new WaitUntil(() => selectedUnit.state == SrpgUnit.State.Spent);
        selectedUnit = null;
        yield break;
    }

    // We don't need input cooldowns or bounds checking.
    private void MoveAiCursor(Vector3 pos){
        audioSource.PlaySound(ESRPGSound.FieldCursor);
        destinationPos = pos;
    }

}