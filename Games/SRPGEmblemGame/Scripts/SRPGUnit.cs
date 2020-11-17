using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Tilemaps;
using ExtensionMethods;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LerpMovement))]
public class SrpgUnit : EntityDamageable {
    [Header("SrpgUnit")]
    public string id;
    public string name;
    public string typeId;
    public string teamId;
    public int maxHp = 10;
    public int hp = 10;
    public int attack = 1;
    public int defense = 1;
    public int attackRange = 1;
    [Tooltip("Movement range of unit, in tiles.")]
    public int moveRange = 3;
    public List<SrpgItem> items;

    // GameObject refs
    private TilemapCollider2D tilemapCollider2D;
    private SpriteRenderer spriteRenderer;
    private SrpgController srpgController;
    private List<SrpgTile> tiles = null;
    private Collider2D collider;
    private SrpgPrefabContainer prefabContainer;

    [SerializeField] private LerpMovement lerpMovement; 

    // Unit position at the start of this turn. Unit will return here if movement is canceled.
    public Vector2? idlePos = null;
    public State state = State.Idle;
    public bool hasAttackedThisTurn = false;

    // Settings
    public bool friendlyFire = false;

    public enum State {
        Idle,
        SelectingMove,
        Moving,
        Moved,
        SelectingAttackTarget,
        Spent,
    }

    private void Awake() {
        base.Awake();
        tilemapCollider2D = FindObjectOfType<TilemapCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lerpMovement = GetComponent<LerpMovement>();
        idlePos = transform.position;
        collider = GetComponent<Collider2D>();
        srpgController = FindObjectOfType<SrpgController>();
        prefabContainer = FindObjectOfType<SrpgPrefabContainer>();
        maxHealth = maxHp;
        health = hp;
    }

    public void ToSelectingMove(){
        lerpMovement.destinationPos = idlePos;
        List<Vector2> movePositions = GetMovePositions();
        DestroyTiles();
        foreach (Vector2 pos in movePositions){
            GameObject tileObj = Instantiate(prefabContainer.pfTileMove, pos, Quaternion.identity);
            SrpgTile tile = tileObj.GetComponent<SrpgTile>();
            tile.parentUnit = this;
            tiles.Add(tile);
        }
        foreach (Vector2 pos2 in GetAttackPositions(movePositions, excludeFrom: true, includeEmpty: true)){
            GameObject tileObj2 = Instantiate(prefabContainer.pfTileAttack, pos2, Quaternion.identity);
            SrpgTile tile2 = tileObj2.GetComponent<SrpgTile>();
            tile2.parentUnit = this;
            tiles.Add(tile2);
        }
        state = State.SelectingMove;
    }

    public void ToSelectingAttackTarget(){
        DestroyTiles();
        foreach (Vector2 pos2 in GetAttackPositions(transform.position, includeEmpty: true)){
            GameObject tileObj2 = Instantiate(prefabContainer.pfTileAttack, pos2, Quaternion.identity);
            SrpgTile tile2 = tileObj2.GetComponent<SrpgTile>();
            tile2.parentUnit = this;
            tiles.Add(tile2);
        }
        state = State.SelectingAttackTarget;
    }

    // Get the positions that this unit can currently move to.
    public List<Vector2> GetMovePositions(){
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
    public List<Vector2> GetAttackPositions(List<Vector2> fromPositions, bool excludeFrom = false, bool includeEmpty = false){
        List<Vector2> attackPositions = fromPositions.SelectMany(pos => GetAttackPositions(pos, includeEmpty)).Distinct().ToList();
        if (excludeFrom){
            return attackPositions.Where(atkPos => !fromPositions.Any(fromPos => atkPos == fromPos)).ToList(); // TODO extract into Utils.
        } else {
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

    // includeEmpty: include empty tiles alonside enemy-occupied tiles.
    public List<Vector2> GetAttackTiles(Vector2 fromPosition, bool includeEmpty = false){
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
        SrpgUnit unit = otherColl.GetComponent<SrpgUnit>();
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
        lerpMovement.destinationPos = idlePos;
    }

    private void Update() {
        lerpMovement.Update();
        if(state == State.Moving && lerpMovement.destinationPos == null){
            FinishMoving();
        }
    }

    public void Move(Vector2 pos){
        lerpMovement.destinationPos = pos;
        FindObjectOfType<SrpgAudioSource>().PlaySound(ESRPGSound.UnitFootsteps);
        state = State.Moving;
    }

    public void Attack(SrpgUnit hoveringUnit){
        if(!friendlyFire && hoveringUnit.teamId == teamId){
            Debug.LogWarning("Cannot attack friend! (Friendly fire is ON).");
            return;
        }
        StartCoroutine(CrAttack(hoveringUnit));
    }

    public IEnumerator CrAttack(SrpgUnit hoveringUnit){
        srpgController.ToggleFieldCursor(false);
        FindObjectOfType<SrpgAudioSource>().PlaySound(ESRPGSound.Attack);
        yield return new WaitForSeconds(0.3f);
        int dmg = CalculateDamage(hoveringUnit);
        hoveringUnit.Damage(new Damage(amount: dmg));
        yield return new WaitForSeconds(0.3f);
        ToSpent();
        srpgController.ToggleFieldCursor(true);
    }

    // TODO very primitive. take into account the attack type, etc.
    private int CalculateDamage(SrpgUnit targetUnit){
        return attack - targetUnit.defense;
    }

    public override void Damage(Damage damage){
        base.Damage(damage);
        hp = health;
        if(hp <= 0){
            StartCoroutine(CrDie());
        }
    }

    private IEnumerator CrDie(){
        // TODO death cutscene if unit is important.
        yield return new WaitForSeconds(0.4f);
        // TODO call srpgController.UpdateTeams with an offset.
    }

    public void FinishMoving(){
        FindObjectOfType<SrpgAudioSource>().PlaySound(ESRPGSound.UnitPrompt);
        state = State.Moved;
        FindObjectOfType<SrpgFieldCursor>().OpenUnitMenu();
    }

    // hard: "true" checks for turn end. "false" prevents recursive calls if we are already changing turns.
    public void ToSpent(bool hard = true){
        DestroyTiles();
        FindObjectOfType<SrpgFieldCursor>(includeInactive: true).selectedUnit = null;
        idlePos = transform.position;
        spriteRenderer.color = Color.gray;
        state = State.Spent;
        if(hard){
            FindObjectOfType<SrpgController>().UpdateTeamsHard();
        }
    }

    public bool CanAttack(){
        return !hasAttackedThisTurn && GetAttackPositions(transform.position).Count() > 0;
    }

    public bool CanAttack(SrpgAttack attack){
        return !hasAttackedThisTurn && attack.range == ManhattanDistance(attack.target); // TODO attack.range.Contains(distance)
    }

    public int ManhattanDistance(SrpgUnit other){
        return (int)(this.transform.position.ManhattanDistance(other.transform.position));
    }

    public bool HasItem(){
        return false;
    }

    public void DestroyTiles(){
        if(tiles != null){
            tiles.ForEach(tile => tile.SelfDestroy());
        }
        tiles = new List<SrpgTile>();
    }

    private int GetAttackRange(){
        return attackRange; // TODO get the max and min of all possible attacks this unit can perform.
        // TODO consider airborne units, etc. Returning an int will not suffice, change to a more complex struct.
        // TODO this struct should have: attack range, attack type, AND attack Kernel (think matrix convolution).
        // X X  XXX
        //  X   X X  X
        // X X  XXX
    }

    private void OnMouseDown(){
        FindObjectOfType<SrpgFieldCursor>().MoveCursorAndConfirm(transform.position);
    }

    public bool IsAlive(){
        return hp > 0;
    }

    // Return the 'best' possible attack this turn (or none).
    public SrpgAttack BestAttack(){
        // TODO for each attack item, do this.
        List<Vector2> movePositions = GetMovePositions();
        List<Vector2> targetedPositions = GetAttackPositions(movePositions);
        List<SrpgUnit> targetedUnits = srpgController.GetUnitColliders()
            .Where(coll => targetedPositions.Any(pos => coll.bounds.Contains(pos)))
            .Select(coll => coll.GetComponent<SrpgUnit>()).ToList();
        if(targetedUnits.IsEmpty()){
            return null;
        }
        int minHp = targetedUnits.Min(targetUnit => targetUnit.hp);
        SrpgUnit chosenTarget = targetedUnits.First(targetUnit => targetUnit.hp == minHp);
        return new SrpgAttack(){attacker = this, target = chosenTarget, range = GetAttackRange()}; // TODO use item instead.
    }
}
