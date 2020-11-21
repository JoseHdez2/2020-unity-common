using UnityEngine;
using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SpritePopInOut))]
public class SrpgEnemyCursor : LerpMovement {
    
    private SrpgAudioSource audioSource;
    private SrpgController srpgController;
    public string teamId;
    public SrpgUnit selectedUnit;
    private SpritePopInOut spritePopInOut;

    private void Awake() {
        audioSource = FindObjectOfType<SrpgAudioSource>();
        srpgController = FindObjectOfType<SrpgController>();
        spritePopInOut = GetComponent<SpritePopInOut>();
    }

    public void SelfDisable(){
        spritePopInOut.SelfDisable();
    }

    public void StartTurn(){
        StartCoroutine(CrTurn());
    }

    private IEnumerator CrTurn(){
        yield return new WaitForSeconds(1f);
        List<SrpgUnit> units = FindObjectsOfType<SrpgUnit>().ToList().Where(u => u.state == SrpgUnit.State.Idle).ToList();
        while(units.Count > 0) {
            selectedUnit = units[0];
            units.RemoveAt(0);
            StartCoroutine(CrUnitTurn(selectedUnit));
            yield return new WaitUntil(() => selectedUnit == null);
        }
    }

    private IEnumerator CrUnitTurn(SrpgUnit unit){
        yield return StartCoroutine(CrMoveAiCursor(unit.transform.position));
        SrpgAttack bestAttack = unit.MaxDamageAttack();
        audioSource.PlaySound(ESrpgSound.UnitPrompt);
        if(bestAttack == null){
            Log("Won't do anything!");
            selectedUnit.ToSpent();
        } else {
            if(!unit.transform.position.Equals(bestAttack.attackerPos)){
                Log("Will move, then attack!");
                unit.ToSelectingMove();
                yield return new WaitForSeconds(0.5f);
                Vector2 pos = bestAttack.attackerPos;
                yield return StartCoroutine(CrMoveAiCursor(pos, simulateConfirm: true));
                unit.Move(pos);
                yield return new WaitUntil(() => selectedUnit.state != SrpgUnit.State.Moving);
            }
            audioSource.PlaySound(ESrpgSound.UnitPrompt);
            unit.ToSelectingAttackTarget();
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(CrMoveAiCursor(bestAttack.target.transform.position, simulateConfirm: true));
            Log("Will attack!");
            unit.Attack(bestAttack);
        }
        yield return new WaitUntil(() => selectedUnit.state == SrpgUnit.State.Spent);
        selectedUnit = null;
        yield break;
    }

    private void Log(string str){
        Debug.LogFormat($"<color=blue>{str}</color>");
    }

    // We don't need input cooldowns or bounds checking.
    private IEnumerator CrMoveAiCursor(Vector3 pos, bool simulateConfirm= false){
        audioSource.PlaySound(ESrpgSound.FieldCursor);
        destinationPos = pos;
        yield return new WaitUntil(() => !destinationPos.HasValue);
        if(simulateConfirm){
            SimulateConfirm();
        }
    }

    private void SimulateConfirm(){
        audioSource.PlaySound(ESrpgSound.FieldCursor);
        // spritePopInOut.SelfHide();
    }

}