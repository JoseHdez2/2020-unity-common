using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemTypeBase {
    public int id;
    public string uuid;
    public string name;
    public string description;
}

[System.Serializable]
public class ItemStackable {
    public int typeId;
    public string typeUuid;
    public int amount = 1;
}
