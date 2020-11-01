using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemTypeBase {
    public string typeId;
    public string name;
    public string description;
}

[System.Serializable]
public class ItemStackable {
    public string typeId; // string id is inefficient, but flexible. works for me.
    public int amount = 1;
}
