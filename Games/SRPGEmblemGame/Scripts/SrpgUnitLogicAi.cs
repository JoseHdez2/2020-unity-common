using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public static class SrpgUnitLogicAi {
    
    
    // Return the 'best' possible attack this turn (or none).
    public static SrpgAttack MaxDamageAttack(this SRPGUnit unit){
        List<Vector2> movePositions = unit.GetMovePositions();
        List<SrpgAttack> bestAttackForEachWeapon = unit.items.Select(it => MaxDamageAttackForWeapon(unit, it, movePositions)).ToList();
        if(bestAttackForEachWeapon.All(atk => atk == null)){
            return null;
        }
        bestAttackForEachWeapon = bestAttackForEachWeapon.Where(atk => atk != null).ToList();
        int maxDamage = bestAttackForEachWeapon.Max(atk => atk.expectedDamage);
        return bestAttackForEachWeapon.First(atk => atk.expectedDamage == maxDamage);
    }

    private static SrpgAttack MaxDamageAttackForWeapon(this SRPGUnit unit, SrpgItem item, List<Vector2> movePositions) {
        SrpgItemType weaponType = unit.srpgController.database.itemTypes[item.typeId];
        List<Vector2> targetedPositions = unit.GetPossibleTargets(movePositions, range: weaponType.range);
        List<SRPGUnit> targetedUnits = unit.srpgController.GetUnitColliders()
            .Where(coll => targetedPositions.Any(pos => coll.bounds.Contains(pos)))
            .Select(coll => coll.GetComponent<SRPGUnit>()).ToList();
        if(targetedUnits.IsEmpty()){
            return null;
        }
        int minHp = targetedUnits.Min(targetUnit => targetUnit.hp);
        SRPGUnit chosenTarget = targetedUnits.First(targetUnit => targetUnit.hp == minHp);
        SrpgAttack attack = new SrpgAttack(attacker: unit, attackerPos: unit.transform.position, target: chosenTarget, weapon: item, weaponType: weaponType);
        attack.attackerPos = unit.ValidAttackPositions(attack, movePositions).FirstOrDefault();
        attack.expectedDamage = attack.CalculateDamage();
        return attack;
    }

}