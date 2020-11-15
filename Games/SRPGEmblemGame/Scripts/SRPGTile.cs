using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class SrpgTile : SpritePopInOut
{
    public SrpgUnit unit;
    public Highlight highlightType;
    
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

    private void OnMouseDown(){
        FindObjectOfType<SrpgFieldCursor>().MoveCursorAndConfirm(transform.position);
    }
}
