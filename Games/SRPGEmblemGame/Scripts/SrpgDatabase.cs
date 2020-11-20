using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SrpgDatabase : MonoBehaviour {
    
    [SerializeField] private TextAsset databaseJsonFile;
    public SrpgData db;
    public Dictionary<string, SrpgItemType> itemTypes { get; private set; }
    public Dictionary<string, SrpgUnitType> unitTypes { get; private set; }

    protected void Start() {
        db = JsonUtility.FromJson<SrpgData>(databaseJsonFile.text);
        itemTypes = db.itemTypes.ToDictionary(item => item.id);
        unitTypes = db.unitTypes.ToDictionary(unit => unit.id);
    }
}

[System.Serializable]
public class SrpgData {
    [SerializeField] public List<SrpgItemType> itemTypes;
    [SerializeField] public List<SrpgUnitType> unitTypes;
}

[System.Serializable]
public class SrpgItemType {
    public string id;
    public string name;
    public List<int> range;
    public int power;
    public int durability;
}

[System.Serializable]
public class SrpgUnitType {
    public string id;
    public string name;
}