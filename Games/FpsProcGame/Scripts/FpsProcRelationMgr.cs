using System.Collections.Generic;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class FpsProcRelationship {
    public enum Type {boss, subordinate, coworker, spouse, friend, acquaintance};
    public Type type;
    public string uuidA, uuidB;
    public static Type RandomType() => RandomUtils.RandomEnumValue<Type>();

    public FpsProcRelationship(string nameA, string nameB){
        this.type = RandomType();
        this.uuidA = nameA;
        this.uuidB = nameB;
    }
}

public class FpsProcRelationMgr : MonoBehaviour {
    private FpsProcNpc target;
    [SerializeField] List<FpsProcRelationship> relations;
    private FpsProcGameMgr gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<FpsProcGameMgr>();
    }

    public void GenerateRelationships(){
        List<FpsProcNpc> npcs = new List<FpsProcNpc>();
        npcs.Add(target);
        List<FpsProcNpc> unusedNpcs = gameManager.npcs;
        while(npcs.Count > 0){
            FpsProcNpc npc1 = npcs[0];
            npcs.RemoveAt(0);
            FpsProcNpc npc2 = unusedNpcs.RandomItem();
            unusedNpcs.Remove(npc2);
            FpsProcRelationship r = new FpsProcRelationship(npc1.data.fullName, npc2.data.fullName);
            relations.Add(r);
            npcs.Add(npc2);
        }
    }
}