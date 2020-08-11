using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillTree<ESkill> : MonoBehaviour
{
    public SerializableDictionaryBase<ESkill, Button> buttons;
    public ESkill skillType;
    public TMP_Text textSkillPoints;
    private TMP_Text btnText;

    public abstract SkillDatabase<ESkill> GetSkillDb();

    void Start()
    {
        buttons.Keys.ToList().ForEach(skill => InitializeSkillButton(skill, buttons[skill]));
    }

    private void InitializeSkillButton(ESkill skillType, Button button)
    {
        button.onClick.AddListener(() => Unlock(button));
        btnText = GetComponentInChildren<TMP_Text>();
        btnText.text = skillType.ToString();
        textSkillPoints.text = $"Skill points: {GetSkillDb().skillPoints}";
        if (GetSkillDb().unlockedSkills.Contains(skillType)){
            button.interactable = false;
        }
    }
    
    private void Unlock(Button button)
    {
        Debug.Log("click");
        if (!GetSkillDb().HasRequiredSkills(skillType)){
            Debug.Log("Required skills not owned!");
            return;
        } else if (!GetSkillDb().HasRequiredSkillPoints(skillType)){
            Debug.Log("Not enough skill points to unlock!");
            return;
        } else {
            GetSkillDb().skillPoints -= GetSkillDb().requirements[skillType].requiredSkillPoints;
            textSkillPoints.text = $"Skill points: {GetSkillDb().skillPoints}";
            GetSkillDb().unlockedSkills.Add(skillType);
            button.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
