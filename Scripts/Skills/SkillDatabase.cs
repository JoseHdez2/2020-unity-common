using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SkillRequirements<ESkill>
{
    public List<ESkill> requiredSkills;
    public int requiredSkillPoints;
}

[Serializable]
public class SkillDatabase<ESkill> : ScriptableObject
{
    private enum Saved { skillPoints };
    public List<ESkill> unlockedSkills;
    public SerializableDictionaryBase<ESkill, SkillRequirements<ESkill>> requirements;
    [SerializeField] private int initialSkillPoints;
    public int skillPoints;

    public void Initialize(){
        skillPoints = PlayerPrefs.HasKey(Saved.skillPoints.ToString()) ? PlayerPrefs.GetInt(Saved.skillPoints.ToString()) : initialSkillPoints;
    }

    public bool HasRequiredSkills(ESkill skill) =>
        requirements[skill].requiredSkills.All(c => unlockedSkills.Contains(c));

    public bool HasRequiredSkillPoints(ESkill skill) =>
        skillPoints >= requirements[skill].requiredSkillPoints;
}