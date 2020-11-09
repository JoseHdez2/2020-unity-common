using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class SRPGUnit : LerpMovement
{
    public string id;
    public string name;
    public string typeId;
    public int hp = 10;
    public int attack = 1;
    public int defense = 1;
    [Header("Movement range of unit, in tiles.")]
    public int moveRange = 3;

    private TilemapCollider2D tilemapCollider2D;

    public Vector2? idlePos = null;

    public State state = State.Idle;

    public enum State {
        Idle,
        SelectingMove,
        Moving,
        Moved,
        Spent
    }

    private void Start() {
        tilemapCollider2D = FindObjectOfType<TilemapCollider2D>();
        idlePos = transform.position;
    }

    public void SpawnMoveTiles(GameObject pfTile){
        for (int i = -moveRange; i < moveRange; i++) {
            for (int j = -moveRange; j < moveRange; j++) {
                if (!(i == 0 && j == 0) && (Math.Abs(i) + Math.Abs(j) <= moveRange)){
                    var pos = transform.position + new Vector3(i, j, 0);
                    if(!tilemapCollider2D.OverlapPoint(pos)){
                        var tileObj = Instantiate(pfTile, pos, Quaternion.identity);
                        var tile = tileObj.GetComponent<SRPGTile>();
                    }
                }
            }
        }
        state = State.SelectingMove;
    }

    public void ToIdle(){
        DestroyTiles();
        FindObjectOfType<SRPGFieldCursor>(includeInactive: true).selectedUnit = null;
        state = State.Idle;
        destinationPos = idlePos;
    }

    private void Update() {
        base.Update();
        if(state == State.Moving && destinationPos == null){
            FinishMoving();
        }
    }

    public void Move(Vector2 pos){
        destinationPos = pos;
        FindObjectOfType<SRPGAudioSource>().PlaySound(ESRPGSound.UnitFootsteps);
        state = State.Moving;
    }

    public void FinishMoving(){
        FindObjectOfType<SRPGAudioSource>().PlaySound(ESRPGSound.UnitPrompt);
        state = State.Moved;
        FindObjectOfType<SRPGFieldCursor>().OpenUnitMenu();
    }

    public void ToSpent(){
        idlePos = transform.position;
        state = State.Spent;
    }

    public bool InAttackRange(){
        return false;
    }

    public bool HasItem(){
        return false;
    }

    public void DestroyTiles(){
        FindObjectsOfType<SRPGTile>().ToList().ForEach(i => Destroy(i.gameObject, 1));
    }
}
