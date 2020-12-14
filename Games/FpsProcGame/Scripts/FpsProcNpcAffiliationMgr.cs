using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class FpsProcOrganization {
    public enum Type {School, University, College, Company, Association, Family, Resistance, Government, Army, Church};
    public Type type;
    public string name;
    public int logoIndex;
    // each member uuid alongside their importance (0 is the most important). Doubles as a member list.
    [SerializeField] private SerializableDictionaryBase<string, int> members = new SerializableDictionaryBase<string, int>(); 
    public List<int> membersPerLevel;
    public List<FpsProcBounds> areas = new List<FpsProcBounds>();
    // public SerializableDictionaryBase<string, string> roleNames;
    public static Type RandomType() => RandomUtils.RandomEnumValue<Type>();
    public List<string> GetMembers() => members.Keys.ToList();
    public void AddMember(string npcUuid, int rank) => members[npcUuid] = rank;
    public bool IsMember(string npcUuid) => members.Keys.Contains(npcUuid);
    public int GetRank(string npcUuid) => members[npcUuid];
    public string GetRankNameForMember(string npcUuid) => GetRankName(members[npcUuid]);
    public string GetRankName(int rank) => rankNames[type].GetOrClamp(rank);
    public static Dictionary<Type, List<string>> rankNames = new Dictionary<Type, List<string>>()
    {
        {Type.School, new List<string>(){"Principal", "Teacher", "Student"}},
        {Type.University, new List<string>(){"Chancellor", "Professor", "Associate Professor", "Doctorate Student", "Graduate Student", "Student"}},
        {Type.College, new List<string>(){"Dean", "Professor", "Associate Professor", "Lecturer", "Graduate Student", "Student"}},
        {Type.Government, new List<string>(){"President", "Minister", "Senator", "Ambassador", "Governor"}}, // TODO maybe not correct, w/e.
        {Type.Company, new List<string>(){"CEO", "Director", "Manager", "Assistant Manager", "Employee"}},
        {Type.Association, new List<string>(){"Boss", "Underboss", "Street boss", "Consigliere", "Capo", "Soldier"}},
        {Type.Family, new List<string>(){"Boss", "Underboss", "Street boss", "Consigliere", "Capo", "Soldier"}},
        {Type.Resistance, new List<string>(){"General", "Colonel", "Major", "Captain", "Lieutenant", "Private"}},
        {Type.Army, new List<string>(){"General", "Colonel", "Major", "Captain", "Lieutenant", "Private"}},
        // {Type.Navy, new List<string>(){"Admiral", "Colonel", "Major", "Captain", "Lieutenant", "Private"}},
        // {Type.Family, new List<string>(){"Head", "Grandparent", "Parent", "Child"}},
        {Type.Church, new List<string>(){"Pope", "Cardinal", "Archbishop", "Bishop", "Priest", "Deacon"}},
    };
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
                    List<FpsProcNpc> unemployedNpcs = npcs.Where(n => newOrganizations.None(o => o.IsMember(n.data.uuid))).ToList();
                    if(unemployedNpcs.IsEmpty()){ break; }
                    FpsProcNpc randomNpc = npcs.Where(n => !org.IsMember(n.data.uuid)).ToList().RandomItem();
                    // Debug.Log($"{randomNpc.data.uuid} is a {org.GetRankName(i)} from {org.name}.");
                    org.AddMember(randomNpc.data.uuid, i);
                }
            }
        }
        return newOrganizations;
    }

    public List<FpsProcOrganization> GetOrganizations(string npcUuid){
        return organizations.Where(org => org.IsMember(npcUuid)).ToList();
    }
}