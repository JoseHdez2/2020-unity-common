using System.Collections.Generic;

[System.Serializable]
public class SrpgUnitData {
    public string id;
    public string name;
    public string typeId;
    public string teamId;
    public int maxHp = 10;
    public int hp = 10;
    public int attack = 1;
    public int defense = 1;
    public int moveRange = 3;
    public List<string> items;
}