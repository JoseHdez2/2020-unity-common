using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SrpgTile : SpritePopInOut
{
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
}
