
using ExtensionMethods;
using UnityEngine;

[System.Serializable]
public class SrpgItem {
    public string id;
    public string typeId;
    public string ownerId;
    public int remainingDurability;
}

public class SrpgAttack {
    public SRPGUnit attacker;
    public Vector2 attackerPos; // Note: we don't get it from "attacker" because it may represent a future pos.
    public SRPGUnit target;
    public SrpgItem weapon;
    public SrpgItemType weaponType;
    public int expectedDamage;

    public SrpgAttack(SRPGUnit attacker, Vector2 attackerPos, SRPGUnit target, SrpgItem weapon, SrpgItemType weaponType){
        this.attacker = attacker;
        this.attackerPos = attackerPos;
        this.target = target;
        this.weapon = weapon;
        this.weaponType = weaponType;
    }

    public bool IsValid(){
        int distanceToTarget = attackerPos.ManhattanDistanceInt(target.transform.position);
        return weaponType.range.Contains(distanceToTarget);
    }

    public bool SimulateAttackHit(){
        return true;
    }

    // TODO very primitive. take into account the attack type, etc.
    public int CalculateDamage(){
        return weaponType.power - target.defense; // TODO use unit attack too.
    }
}