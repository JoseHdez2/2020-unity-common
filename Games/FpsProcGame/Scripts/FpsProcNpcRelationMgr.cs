using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class FpsProcRelationship {
    public enum Type {superior, subordinate, coworker, spouse, friend, acquaintance};
    public Type type;
    public string uuidA, uuidB;
    public static Type RandomType() => RandomUtils.RandomEnumValue<Type>();

    public FpsProcRelationship(string nameA, string nameB){
        this.type = RandomType();
        this.uuidA = nameA;
        this.uuidB = nameB;
    }
}

public class FpsProcNpcRelationMgr : MonoBehaviour {
    private FpsProcNpc target;
    private FpsProcGameMgr gameManager;
    public List<FpsProcRelationship> relations;

    [SerializeField] RangeInt relationshipsPerNpc;

    private void Awake() {
        gameManager = FindObjectOfType<FpsProcGameMgr>();
    }

    public List<FpsProcRelationship> GenerateRelationships(List<FpsProcNpc> npcs, FpsProcNpc finalNpc = null){
        List<FpsProcNpc> npcsToRelate = new List<FpsProcNpc>();
        List<FpsProcRelationship> relationships = new List<FpsProcRelationship>();
        npcsToRelate.Add(finalNpc);
        List<FpsProcNpc> unusedNpcs = new List<FpsProcNpc>(npcs);
        unusedNpcs.Remove(finalNpc);
        while(!npcsToRelate.IsEmpty() && !unusedNpcs.IsEmpty()){
            FpsProcNpc npc1 = npcsToRelate[0];
            npcsToRelate.RemoveAt(0);
            FpsProcNpc npc2 = unusedNpcs.RandomItem();
            unusedNpcs.Remove(npc2);
            FpsProcRelationship r = new FpsProcRelationship(npc1.data.fullName, npc2.data.fullName);
            relationships.Add(r);
            npcsToRelate.Add(npc2);
        }
        return relationships;
    }

    public FpsProcRelationship GetRelationship(string npcNameA, string npcNameB){
        return relations.Where(r => r.uuidA == npcNameA && r.uuidB == npcNameB || r.uuidA == npcNameB && r.uuidB == npcNameA).FirstOrDefault();
    }
}