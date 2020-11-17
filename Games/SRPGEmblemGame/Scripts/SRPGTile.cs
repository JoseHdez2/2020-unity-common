using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class SrpgTile : SpritePopInOut
{
    public SrpgUnit parentUnit;
    // public SrpgUnit? unitWithin;
    public Highlight highlightType;
    public Content content;
    
    public enum Highlight {
        Move,
        Attack
    }

    public enum Content {
        Empty,
        Solid,
        HasFriend,
        HasEnemy,
        HasMe
    }

    // TODO
    public class TerrainType {
        public int defense;
        public Dictionary<string, int> traversabilityByUnitType;
    }

    // private void OnEnable() {
    //     var coll = GetComponent<Collider2D>();
    //     unitWithin = FindObjectsOfType<SrpgUnit>().FirstOrDefault(u => coll.bounds.Contains(u.transform.position));
    //     if(!unitWithin.HasValue){
    //         content = Content.Empty
    //     } else {
            
    //     }
    // }

    private void OnMouseDown(){
        FindObjectOfType<SrpgFieldCursor>().MoveCursorAndConfirm(transform.position);
    }
}
