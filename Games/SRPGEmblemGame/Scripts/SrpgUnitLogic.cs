using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SrpgUnitLogic {

    // Get the positions that this unit can currently move to.
    public static List<Vector2> GetMovePositions(this SrpgUnit unit){
        List<Vector2> movePositions = new List<Vector2>();
        List<BoxCollider2D> unitColls = unit.srpgController.GetUnitColliders();
        for (int i = -unit.moveRange; i <= unit.moveRange; i++) {
            for (int j = -unit.moveRange; j <= unit.moveRange; j++) {
                if(Math.Abs(i) + Math.Abs(j) > unit.moveRange){ continue; }
                Vector3 pos = unit.transform.position + new Vector3(i, j, 0);
                var tileType = unit.GetTileType(pos, unitColls);
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
    public static List<Vector2> GetAttackPositions(this SrpgUnit unit, List<Vector2> fromPositions, bool excludeFrom = false, bool includeEmpty = false, SrpgItemType weapon = null){
        List<Vector2> attackPositions = fromPositions.SelectMany(pos => unit.GetAttackPositions(pos, includeEmpty, weapon)).Distinct().ToList();
        if (excludeFrom){
            return attackPositions.Where(atkPos => !fromPositions.Any(fromPos => atkPos == fromPos)).ToList(); // TODO extract into Utils.
        } else {
            return attackPositions;
        }
    }

    // includeEmpty: include empty tiles alonside enemy-occupied tiles.
    private static List<Vector2> GetAttackPositions(this SrpgUnit unit, Vector2 fromPosition, bool includeEmpty = false, SrpgItemType weapon = null){
        List<Vector2> attackPositions = new List<Vector2>();
        List<BoxCollider2D> unitColls = unit.srpgController.GetUnitColliders();
        List<int> attackRange = (weapon != null) ? weapon.range : GetAttackRange(unit);
        for (int i = -attackRange.Max(); i <= attackRange.Max(); i++) {
            for (int j = -attackRange.Max(); j <= attackRange.Max(); j++) {
                if(!attackRange.Contains(Math.Abs(i) + Math.Abs(j))){ continue; }
                Vector3 pos = new Vector3(fromPosition.x + i, fromPosition.y + j, 0);
                var tileType = unit.GetTileType(pos, unitColls);
                if(tileType == SrpgTile.Content.HasEnemy || (includeEmpty && tileType == SrpgTile.Content.Empty)){
                    attackPositions.Add(pos);
                }
            }
        }
        return attackPositions;
    }

    public static List<int> GetAttackRange(this SrpgUnit unit){
        return unit.items.Select(item => unit.srpgController.database.itemTypes[item.typeId])
            .Select(type => type.range)
            .Aggregate((a,b) => {a.AddRange(b); a.Sort(); return a;})
            .Distinct().ToList(); 
        // TODO consider airborne units, etc. Returning an int will not suffice, change to a more complex struct.
        // TODO this struct should have: attack range, attack type, AND attack Kernel (think matrix convolution).
        // X X  XXX
        //  X   X X  X
        // X X  XXX
    }
    
    public static List<Vector2> ValidAttackPositions(this SrpgUnit unit, SrpgAttack attack, List<Vector2> movePositions){
        Vector2 targetPos = attack.target.transform.position;
        SrpgItemType weaponType = unit.srpgController.database.itemTypes[attack.weapon.typeId];
        return movePositions.Where(pos => weaponType.range.Contains((pos.ManhattanDistanceInt(targetPos)))).ToList();
    }


    public static bool CanAttack(this SrpgUnit unit){
        return !unit.hasAttackedThisTurn && unit.GetAttackPositions(unit.transform.position).Count() > 0;
    }

    private static SrpgTile.Content GetTileType(this SrpgUnit unit, Vector3 pos, List<BoxCollider2D> unitColls){
        if(unit.tilemapCollider2D.OverlapPoint(pos)){
            return SrpgTile.Content.Solid;
        }
        if(unit.collider.bounds.Contains(pos)){
            return SrpgTile.Content.HasMe;
        }
        BoxCollider2D otherColl = unitColls.FirstOrDefault(coll => coll.bounds.Contains(pos));
        if(otherColl == null){
            return SrpgTile.Content.Empty;
        }
        SrpgUnit otherUnit = otherColl.GetComponent<SrpgUnit>();
        if(otherUnit.teamId == unit.teamId){
            return SrpgTile.Content.HasFriend;
        } else {
            return SrpgTile.Content.HasEnemy;
        }
    }
}