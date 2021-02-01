using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Tilemaps;
using ExtensionMethods;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LerpMovement))]
public class SRPGUnit : EntityDamageable {
    [Header("SrpgUnit")]

    public SrpgUnitData data;
    
    public string name => data.name;
    public string teamId => data.teamId;
    public string typeId => data.typeId;
    public int hp => data.hp;
    public int maxHp => data.maxHp;
    public int defense => data.defense;
    public int moveRange => data.moveRange;
    public List<SrpgItem> items;

    public Sprite attackSprite;

    // GameObject refs
    public TilemapCollider2D tilemapCollider2D;
    private SpriteRenderer spriteRenderer;
    public SrpgController srpgController;
    private List<SrpgTile> tiles = null;
    public Collider2D collider2d;
    private SrpgPrefabContainer prefabContainer;
    private SrpgAudioSource srpgAudioSource;

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
        SelectingAttackType,
        Spent,
    }

    private new void Awake() {
        base.Awake();
        tilemapCollider2D = FindObjectOfType<TilemapCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        lerpMovement = GetComponent<LerpMovement>();
        srpgAudioSource = FindObjectOfType<SrpgAudioSource>();
        idlePos = transform.position;
        collider2d = GetComponent<Collider2D>();
        srpgController = FindObjectOfType<SrpgController>(includeInactive: true);
        prefabContainer = FindObjectOfType<SrpgPrefabContainer>();
        maxHealth = data.maxHp;
        health = data.hp;
    }

    public void ToSelectingMove(){
        state = State.SelectingMove;
        lerpMovement.destinationPos = idlePos;
        List<Vector2> movePositions = this.GetMovePositions();
        List<Vector2> attackPositions = this.GetPossibleTargets(origins: movePositions, excludeFrom: true, includeEmpty: true);
        DestroyTiles();
        StartCoroutine(CrCreateTiles(movePositions, prefabContainer.pfTileMove));
        StartCoroutine(CrCreateTiles(attackPositions, prefabContainer.pfTileAttack));
    }

    internal void SetData(SrpgUnitData unitData)
    {
        data = unitData;
    }

    public void ToSelectingAttackTarget(){
        state = State.SelectingAttackTarget;
        DestroyTiles();
        List<Vector2> attackPositions = this.GetPossibleTargets(this.GetCurPos(), includeEmpty: true);
        StartCoroutine(CrCreateTiles(attackPositions, prefabContainer.pfTileAttack));
    }

    public void ToSelectingAttackType(){
        state = State.SelectingAttackType;
        // FindObjectOfType
    }

    private IEnumerator CrCreateTiles(List<Vector2> positions, GameObject pfTile){
        foreach (Vector2 pos in positions){
            GameObject tileObj = Instantiate(pfTile, pos, Quaternion.identity);
            SrpgTile tile = tileObj.GetComponent<SrpgTile>();
            tile.parentUnit = this;
            tiles.Add(tile);
            yield return new WaitForSeconds(0.005f);
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
        srpgAudioSource.PlaySound(ESrpgSound.UnitFootsteps);
        state = State.Moving;
    }

    public void Attack(SrpgAttack attack){
        if(!friendlyFire && attack.target.data.teamId == attack.attacker.data.teamId){
            Debug.LogWarning("Cannot attack friend! (Friendly fire is ON).");
            return;
        }
        StartCoroutine(CrAttack(attack.target, attack));
    }

    public IEnumerator CrAttack(SRPGUnit targetedUnit, SrpgAttack attack, bool canCounter= true){
        srpgController.ToggleFieldCursorFalse();
        yield return new WaitForSeconds(0.3f); // hack so that the Attack sound is played. Otherwise the "menu close" sound overrides.
        srpgAudioSource.PlaySound(ESrpgSound.Attack);
        if(attackSprite){
            spriteRenderer.sprite = attackSprite;
        }
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = idleSprite;
        if(attack.SimulateAttackHit()){
            SrpgItem usedWeapon = items.FirstOrDefault(it => it.id == attack.weapon.id);
            usedWeapon.remainingDurability -= 1;
            if(usedWeapon.remainingDurability <= 0){
                Debug.Log("Weapon broke!"); // TODO
            }
            int dmg = attack.CalculateDamage();
            targetedUnit.Damage(new Damage(amount: dmg));
        } else {
            srpgAudioSource.PlaySound(ESrpgSound.Miss);
        }
        yield return new WaitForSeconds(0.3f);
        if(canCounter && targetedUnit.IsAlive() && targetedUnit.CanAttackWithFirstWeapon(this)){
            SrpgAttack counterAtk = new SrpgAttack(targetedUnit, targetedUnit.transform.position, target: this, targetedUnit.items[0], this.GetType(targetedUnit.items[0]));
            yield return StartCoroutine(targetedUnit.CrAttack(this, counterAtk, canCounter: false));
        }
        ToSpent();
    }

    public override void Damage(Damage damage){
        base.Damage(damage);
        data.hp = health;
        if(data.hp <= 0){
            StartCoroutine(CrDie());
        }
    }

    private IEnumerator CrDie(){
        // TODO death cutscene if unit is important.
        yield return new WaitForSeconds(0.4f);
        // TODO call srpgController.UpdateTeams with an offset.
    }

    public void FinishMoving(){
        srpgAudioSource.PlaySound(ESrpgSound.UnitPrompt);
        state = State.Moved;
        if(data.teamId == "good guys"){
            FindObjectOfType<SrpgFieldCursor>().OpenUnitMenu();
        }
    }

    // hard: "true" checks for turn end. "false" prevents recursive calls if we are already changing turns.
    public void ToSpent(bool checkForTurnChange = true){
        DestroyTiles();
        FindObjectOfType<SrpgFieldCursor>(includeInactive: true).selectedUnit = null;
        idlePos = transform.position;
        spriteRenderer.color = Color.gray;
        state = State.Spent;
        if(checkForTurnChange){
            FindObjectOfType<SrpgController>().UpdateTeamsAndCheckForTurnChange();
        }
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


    private void OnMouseDown(){
        FindObjectOfType<SrpgFieldCursor>().MoveCursorAndConfirm(transform.position);
    }

    public bool IsAlive(){
        return data.hp > 0;
    }

}
