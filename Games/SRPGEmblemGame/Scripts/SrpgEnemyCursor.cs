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
        MoveAiCursor(unit.transform.position);
        yield return new WaitUntil(() => !destinationPos.HasValue);
        SrpgAttack bestAttack = unit.MaxDamageAttack();
        if(bestAttack == null){
            Log("Won't do anything!");
            audioSource.PlaySound(ESRPGSound.UnitPrompt);
            selectedUnit.ToSpent();
        } else {
            if(!bestAttack.IsValid()){
                Log("Will move, then attack!");
                unit.ToSelectingMove();
                yield return new WaitForSeconds(0.5f);
                Vector2 pos = bestAttack.attackerPos;
                unit.Move(pos);
                yield return new WaitUntil(() => selectedUnit.state != SrpgUnit.State.Moving);
                MoveAiCursor(pos);
                yield return new WaitUntil(() => destinationPos == null);
            }
            audioSource.PlaySound(ESRPGSound.UnitPrompt);
            unit.ToSelectingAttackTarget();
            yield return new WaitForSeconds(0.5f);
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
    private void MoveAiCursor(Vector3 pos){
        audioSource.PlaySound(ESRPGSound.FieldCursor);
        destinationPos = pos;
    }

}