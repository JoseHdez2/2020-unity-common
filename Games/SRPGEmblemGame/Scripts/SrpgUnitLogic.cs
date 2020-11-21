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
        return GetPositions(range: unit.GetMoveRange(), origins: unit.GetCurPos(), predicate: (pos) => unit.IsOccupiableTile(pos, unitColls));
    }

    public static List<int> GetMoveRange(this SrpgUnit unit){
        return Enumerable.Range(0, unit.moveRange + 1).ToList();
    }
    
    public static List<Vector2> GetCurPos(this SrpgUnit unit){
        return new List<Vector2>(){unit.transform.position};
    }

    private static bool IsOccupiableTile(this SrpgUnit unit, Vector3 pos, List<BoxCollider2D> unitColls) {
        switch(GetTileType(unit, pos, unitColls)){
            case SrpgTile.Content.HasMe:
            case SrpgTile.Content.Empty:
                return true;
            default:
                return false;
        }
    }

    public static List<Vector2> GetPossibleTargets(this SrpgUnit unit){
        return unit.GetPossibleTargets(origins: unit.GetCurPos(), excludeFrom: false, unit.GetAttackRange(), includeEmpty: false);
    }

    public static List<Vector2> GetPossibleTargets(this SrpgUnit unit, List<Vector2> origins, bool excludeFrom = false, List<int> range = null, bool includeEmpty = false){
        List<Vector2> attackPositions = new List<Vector2>();
        List<BoxCollider2D> unitColls = unit.srpgController.GetUnitColliders();
        if(range == null){
            range = unit.GetAttackRange();
        }
        List<Vector2> targetPositions = GetPositions(range, origins, (pos) => unit.IsEnemyTile(pos, unitColls, includeEmpty));
        if(excludeFrom){
            targetPositions.RemoveAll(targetPos => origins.Contains(targetPos));
        }
        return targetPositions;
    }

    private static List<Vector2> GetPositions(List<int> range, List<Vector2> origins, Predicate<Vector2> predicate){
        return origins.SelectMany(o => GetPositions(range, o, pos => true)).Distinct().ToList().FindAll(predicate);
    }
    // TODO use "reduce" to improve perf?

    private static List<Vector2> GetPositions(List<int> range, Vector2 origin, Predicate<Vector2> predicate){
        List<Vector2> positions = new List<Vector2>();
        for (int i = -range.Max(); i <= range.Max(); i++) {
            for (int j = -range.Max(); j <= range.Max(); j++) {
                var v = new Vector2(origin.x + i, origin.y + j);
                if(range.Contains(Math.Abs(i) + Math.Abs(j))){ 
                    positions.Add(new Vector2(origin.x + i, origin.y + j)); 
                }
            }
        }
        return positions.FindAll(predicate);
    }

    public static List<int> GetAttackRange(this SrpgUnit unit){
        return unit.items.Select(item => unit.srpgController.database.itemTypes[item.typeId])
            .Select(type => type.range.GetRange(0, type.range.Count()))
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

    public static bool CanAttackSomeTarget(this SrpgUnit unit){
        return !unit.hasAttackedThisTurn && unit.GetPossibleTargets().Count() > 0;
    }

    public static bool CanAttack(this SrpgUnit unit, SrpgUnit targetUnit){
        return !unit.hasAttackedThisTurn && unit.GetPossibleTargets().Contains(targetUnit.transform.position);
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

    private static bool IsEnemyTile(this SrpgUnit unit, Vector3 pos, List<BoxCollider2D> unitColls, bool includeEmpty) {
        switch(GetTileType(unit, pos, unitColls)){
            case SrpgTile.Content.HasEnemy:
                return true;
            case SrpgTile.Content.Empty:
                return includeEmpty;
            default:
                return false;
        }
    }
}