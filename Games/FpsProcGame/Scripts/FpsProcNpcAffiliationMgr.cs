using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class FpsProcOrganization {
    public enum Type {School, University, Company, Association, Resistance, Government, Army, Family};
    public Type type;
    public string name;
    public int logoIndex;
    // each member uuid alongside their importance (0 is the most important).
    public SerializableDictionaryBase<string, int> members = new SerializableDictionaryBase<string, int>(); 
    public List<int> membersPerLevel;
    public List<FpsProcBounds> areas;
    // public SerializableDictionaryBase<string, string> roleNames;
    public static Type RandomType() => RandomUtils.RandomEnumValue<Type>();
    public int GetMembersAmount() => membersPerLevel.Count();
}

[System.Serializable]
public class RangeInt{
    public int min, max;

    public RangeInt() {}
    public RangeInt(int min, int max) { (this.min, this.max) = (min, max);}

    public int RandomInt() => Random.Range(min, max);
}

public class FpsProcNpcAffiliationMgr : MonoBehaviour {
    private FpsProcNpc target;
    private FpsProcGameMgr gameManager;
    public List<FpsProcOrganization> organizations;
    [SerializeField] RangeInt orgsAmount;
    [SerializeField] List<RangeInt> membersPerLevel;

    private void Awake() {
        gameManager = FindObjectOfType<FpsProcGameMgr>();
    }

    public List<FpsProcOrganization> GenerateOrganizations() =>
        Enumerable.Range(0, orgsAmount.RandomInt()).Select(i => GenerateOrganization()).ToList();

    public FpsProcOrganization GenerateOrganization(){
        FpsProcOrganization.Type type = FpsProcOrganization.RandomType();
        return new FpsProcOrganization(){
            type = type,
            name = $"{FpsProcDatabase.streetNames.RandomItem()} {type}",
            membersPerLevel = membersPerLevel.Select(level => level.RandomInt()).ToList(),
            logoIndex = FpsProcDatabase.GetRandomIconIndex()
        };
    }

    public List<FpsProcOrganization> GenerateAffiliations(List<FpsProcOrganization> organizations, List<FpsProcNpc> npcs, FpsProcNpc finalNpc = null){
        List<FpsProcOrganization> newOrganizations = new List<FpsProcOrganization>(organizations);
        foreach(FpsProcOrganization org in newOrganizations){
            for(int i = 0; i < org.membersPerLevel.Count; i++){
                for (int j = 0; j < org.membersPerLevel[i]; j++){
                    List<FpsProcNpc> npcsNotInOrg = npcs.Where(n => !org.members.ContainsKey(n.data.uuid)).ToList();
                    if(npcsNotInOrg.IsEmpty()){ break; }
                    FpsProcNpc randomNpc = npcs.Where(n => !org.members.ContainsKey(n.data.uuid)).ToList().RandomItem();
                    org.members[randomNpc.data.uuid] = i;
                }
            }
        }
        return newOrganizations;
    }

    public int GetAffiliation(string npcUuid, string orgName){
        return organizations.FirstOrDefault(o => o.name == orgName).members[npcUuid];
    }

    public List<FpsProcOrganization> GetOrganizations(string npcUuid){
        return organizations.Where(o => o.members.ContainsKey(npcUuid)).ToList();
    }
}