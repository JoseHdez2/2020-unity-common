using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using ExtensionMethods;

[RequireComponent(typeof(SpriteRenderer))]
public class SRPGUnit : LerpMovement
{
    public string id;
    public string name;
    public string typeId;
    public string teamId;
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

    private void Awake() {
        tilemapCollider2D = FindObjectOfType<TilemapCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        idlePos = transform.position;
    }

    public void SpawnMoveTiles(GameObject pfTile){
        List<BoxCollider2D> unitColls = FindObjectsOfType<SRPGUnit>().Select(unit => unit.GetComponent<BoxCollider2D>()).ToList();
        tiles = new List<SRPGTile>();
        for (int i = -moveRange; i <= moveRange; i++) {
            for (int j = -moveRange; j <= moveRange; j++) {
                if (!(i == 0 && j == 0) && (Math.Abs(i) + Math.Abs(j) <= moveRange)){
                    Vector3 pos = transform.position + new Vector3(i, j, 0);
                    if(IsEmptyTile(pos, unitColls)){
                        GameObject tileObj = Instantiate(pfTile, pos, Quaternion.identity);
                        SRPGTile tile = tileObj.GetComponent<SRPGTile>();
                        tiles.Add(tile);
                    }
                }
            }
        }
        state = State.SelectingMove;
    }

    private bool IsEmptyTile(Vector3 pos, List<BoxCollider2D> unitColls){
        return !tilemapCollider2D.OverlapPoint(pos) && unitColls.None(coll => coll.bounds.Contains(pos));
    }

    public void ToIdle(){
        DestroyTiles();
        FindObjectOfType<SrpgFieldCursor>(includeInactive: true).selectedUnit = null;
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
        FindObjectOfType<SrpgAudioSource>().PlaySound(ESRPGSound.UnitFootsteps);
        state = State.Moving;
    }

    public void FinishMoving(){
        FindObjectOfType<SrpgAudioSource>().PlaySound(ESRPGSound.UnitPrompt);
        state = State.Moved;
        FindObjectOfType<SrpgFieldCursor>().OpenUnitMenu();
    }

    public void ToSpent(){
        DestroyTiles();
        FindObjectOfType<SrpgFieldCursor>(includeInactive: true).selectedUnit = null;
        idlePos = transform.position;
        spriteRenderer.color = Color.gray;
        state = State.Spent;
        FindObjectOfType<SrpgController>().CheckForTurnChange();
    }

    public bool InAttackRange(){
        return false;
    }

    public bool HasItem(){
        return false;
    }

    public void DestroyTiles(){
        if(tiles != null){
            tiles.ForEach(tile => tile.SelfDestroy());
            tiles = null;
        } else {
            Debug.LogWarning("Tried to destroy non-existent tiles.");
        }
    }
}
