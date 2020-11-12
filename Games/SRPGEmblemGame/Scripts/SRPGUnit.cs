﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using ExtensionMethods;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
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
    private SrpgController srpgController;
    private List<SrpgTile> tiles = null;
    private Collider2D collider;

    public Vector2? idlePos = null;
    public State state = State.Idle;
    public bool hasAttackedThisTurn = false;

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
        collider = GetComponent<Collider2D>();
        srpgController = FindObjectOfType<SrpgController>();
    }

    public void SpawnMoveTiles(GameObject pfTileMove, GameObject pfTileAttack){
        List<Vector2> movePositions = GetMovePositions();
        tiles = new List<SrpgTile>();
        foreach (Vector2 pos in movePositions){
            GameObject tileObj = Instantiate(pfTileMove, pos, Quaternion.identity);
            SrpgTile tile = tileObj.GetComponent<SrpgTile>();
            tiles.Add(tile);
        }
        foreach (Vector2 pos2 in GetAttackPositions(movePositions, excludeFrom: true, includeEmpty: true)){
            GameObject tileObj2 = Instantiate(pfTileAttack, pos2, Quaternion.identity);
            SrpgTile tile2 = tileObj2.GetComponent<SrpgTile>();
            tiles.Add(tile2);
        }
        state = State.SelectingMove;
    }

    // Get the positions that this unit can currently move to.
    private List<Vector2> GetMovePositions(){
        List<Vector2> movePositions = new List<Vector2>();
        List<BoxCollider2D> unitColls = srpgController.GetUnitColliders();
        for (int i = -moveRange; i <= moveRange; i++) {
            for (int j = -moveRange; j <= moveRange; j++) {
                if(Math.Abs(i) + Math.Abs(j) > moveRange){ continue; }
                Vector3 pos = transform.position + new Vector3(i, j, 0);
                var tileType = GetTileType(pos, unitColls);
                if(tileType == SrpgTile.Content.Empty || tileType == SrpgTile.Content.HasMe){
                    movePositions.Add(pos);
                }
            }
        }
        return movePositions;
    }

    // Get the positions that this unit can attack before or after moving.
    // excludeFrom: result will not include fromPositions.
    // includeEmpty: include empty tiles alonside enemy-occupied tiles.
    private List<Vector2> GetAttackPositions(List<Vector2> fromPositions, bool excludeFrom = false, bool includeEmpty = false){
        List<Vector2> attackPositions = fromPositions.SelectMany(pos => GetAttackPositions(pos, includeEmpty)).Distinct().ToList();
        if (excludeFrom){
            return attackPositions.Where(atkPos => !fromPositions.Any(fromPos => atkPos == fromPos)).ToList(); // TODO extract into Utils.
        } else {
            Debug.Log("GetAttackPositions return.");
            return attackPositions;
        }
    }

    // includeEmpty: include empty tiles alonside enemy-occupied tiles.
    private List<Vector2> GetAttackPositions(Vector2 fromPosition, bool includeEmpty = false){
        List<Vector2> attackPositions = new List<Vector2>();
        List<BoxCollider2D> unitColls = srpgController.GetUnitColliders();
        int attackRange = GetAttackRange();
        for (int i = -attackRange; i <= attackRange; i++) {
            for (int j = -attackRange; j <= attackRange; j++) {
                if(Math.Abs(i) + Math.Abs(j) > attackRange){ continue; }
                Vector3 pos = new Vector3(fromPosition.x + i, fromPosition.y + j, 0);
                var tileType = GetTileType(pos, unitColls);
                if(tileType == SrpgTile.Content.HasEnemy || (includeEmpty && tileType == SrpgTile.Content.Empty)){
                    attackPositions.Add(pos);
                }
            }
        }
        return attackPositions;
    }

    private SrpgTile.Content GetTileType(Vector3 pos, List<BoxCollider2D> unitColls){
        if(tilemapCollider2D.OverlapPoint(pos)){
            return SrpgTile.Content.Solid;
        }
        if(collider.bounds.Contains(pos)){
            return SrpgTile.Content.HasMe;
        }
        BoxCollider2D otherColl = unitColls.FirstOrDefault(coll => coll.bounds.Contains(pos));
        if(otherColl == null){
            return SrpgTile.Content.Empty;
        }
        SRPGUnit unit = otherColl.GetComponent<SRPGUnit>();
        if(unit.teamId == teamId){
            return SrpgTile.Content.HasFriend;
        } else {
            return SrpgTile.Content.HasEnemy;
        }
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

    public void Attack(Vector2 pos){
        destinationPos = pos;
        FindObjectOfType<SrpgAudioSource>().PlaySound(ESRPGSound.UnitFootsteps);
        // state = State.Moving;
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

    public bool CanAttack(){
        return !hasAttackedThisTurn && GetAttackPositions(transform.position).Count() > 0;
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

    private int GetAttackRange(){
        return 1; // TODO get the max and min of all possible attacks this unit can perform.
        // TODO consider airborne units, etc. Returning an int will not suffice, change to a more complex struct.
        // TODO this struct should have: attack range, attack type, AND attack Kernel (think matrix convolution).
        // X X  XXX
        //  X   X X  X
        // X X  XXX
    }
}
