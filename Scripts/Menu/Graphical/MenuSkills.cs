using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioMultiSource))]
public abstract class MenuSkills<TEnum> : MonoBehaviour
    where TEnum : struct, IConvertible, IComparable, IFormattable
{
    [Tooltip("All skills that exist, at least for this menu.")]
    public TEnum[] skills;
    [Tooltip("Price for each skill, in skill points.")]
    protected Dictionary<TEnum, int> skillPrices;

    [Tooltip("Prerequisite skills for buying the skill.")]
    protected Dictionary<TEnum, TEnum[]> skillPrerequisites;
    protected AudioMultiSource audioMultiSource;

    protected void Start(){
        skills = GetAllSkills();
        skillPrices = GetSkillPrices();
    }

    protected bool IsOwned(TEnum skill){
        return GetOwnedSkills().Contains(skill);
    }

    protected void UnlockSkill(TEnum skill){
        if(!IsOwned(skill) && CanBuy(skill)){
            audioMultiSource.PlayYesSound();
            AddToOwnedSkills(skill);
            SetSkillPoints(GetSkillPoints() - skillPrices[skill]);
        } else {
            audioMultiSource.PlayNoSound();
        }
    }

    protected bool CanBuy(TEnum skill){
        return GetSkillPoints() >= skillPrices[skill];
    }

    // Update is called once per frame
    void Update(){
        
    }

    protected abstract TEnum[] GetAllSkills();
    protected abstract Dictionary<TEnum, int> GetSkillPrices();
    protected abstract Dictionary<TEnum, TEnum[]> GetSkillPrerequisites();

    protected abstract int GetSkillPoints();
    protected abstract void SetSkillPoints(int qty);
    protected abstract List<TEnum> GetOwnedSkills();
    protected abstract void AddToOwnedSkills(TEnum skill);

    protected abstract void RefreshMenu();
}
