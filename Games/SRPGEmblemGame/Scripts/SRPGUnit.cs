using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
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

    // GameObject refs
    private TilemapCollider2D tilemapCollider2D;
    private SpriteRenderer spriteRenderer;
    private List<SRPGTile> tiles = null;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        idlePos = transform.position;
    }

    public void SpawnMoveTiles(GameObject pfTile){
        tiles = new List<SRPGTile>();
        for (int i = -moveRange; i < moveRange; i++) {
            for (int j = -moveRange; j < moveRange; j++) {
                if (!(i == 0 && j == 0) && (Math.Abs(i) + Math.Abs(j) <= moveRange)){
                    Vector3 pos = transform.position + new Vector3(i, j, 0);
                    if(!tilemapCollider2D.OverlapPoint(pos)){
                        GameObject tileObj = Instantiate(pfTile, pos, Quaternion.identity);
                        SRPGTile tile = tileObj.GetComponent<SRPGTile>();
                        tiles.Add(tile);
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
        spriteRenderer.color = Color.white;
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
        DestroyTiles();
        FindObjectOfType<SRPGFieldCursor>(includeInactive: true).selectedUnit = null;
        idlePos = transform.position;
        spriteRenderer.color = Color.gray;
        state = State.Spent;
    }

    public bool InAttackRange(){
        return false;
    }

    public bool HasItem(){
        return false;
    }

    public void DestroyTiles(){
        tiles.ForEach(tile => tile.SelfDestroy());
        tiles = null;
    }
}
