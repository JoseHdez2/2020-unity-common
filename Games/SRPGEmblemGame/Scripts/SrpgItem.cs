
[System.Serializable]
public class SrpgItem {
    public string typeId;
    public int amount;
}

public class SrpgItemType {
    public string typeId;
    public string name;
    public int range;
    public int power;
}

public class SrpgAttack {
    public SrpgUnit attacker;
    public SrpgUnit target;
    // SrpgItemType item;
    public int range;
}