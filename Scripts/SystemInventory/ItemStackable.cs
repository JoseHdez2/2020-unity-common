using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType2 {
    int id;
    string name;
    string description;
}

[System.Serializable]
public class ItemStackable {
    public int typeId;
    public int amount = 1;
}
