using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public static class SrpgUnitLogicAi {
    
    
    // Return the 'best' possible attack this turn (or none).
    public static SrpgAttack MaxDamageAttack(this SrpgUnit unit){
        List<Vector2> movePositions = unit.GetMovePositions();
        List<SrpgAttack> bestAttackForEachWeapon = unit.items.Select(it => MaxDamageAttackForWeapon(unit, it, movePositions)).ToList();
        int maxDamage = bestAttackForEachWeapon.Max(atk => atk.expectedDamage);
        return bestAttackForEachWeapon.First(atk => atk.expectedDamage == maxDamage);
    }

    private static SrpgAttack MaxDamageAttackForWeapon(this SrpgUnit unit, SrpgItem item, List<Vector2> movePositions) {
        SrpgItemType weaponType = unit.srpgController.database.itemTypes[item.typeId];
        List<Vector2> targetedPositions = unit.GetPossibleTargets(movePositions);
        List<SrpgUnit> targetedUnits = unit.srpgController.GetUnitColliders()
            .Where(coll => targetedPositions.Any(pos => coll.bounds.Contains(pos)))
            .Select(coll => coll.GetComponent<SrpgUnit>()).ToList();
        if(targetedUnits.IsEmpty()){
            return null;
        }
        int minHp = targetedUnits.Min(targetUnit => targetUnit.hp);
        SrpgUnit chosenTarget = targetedUnits.First(targetUnit => targetUnit.hp == minHp);
        SrpgAttack attack = new SrpgAttack(attacker: unit, attackerPos: unit.transform.position, target: chosenTarget, weapon: item, weaponType: weaponType);
        attack.attackerPos = unit.ValidAttackPositions(attack, movePositions).FirstOrDefault();
        return attack;
    }

}