using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Float
{
    public Float(float value)
    {
        HasValue = true;
        Value = value;
    }



    [SerializeField] public bool HasValue = false;
    [SerializeField]  float Value;
}

[Serializable]
[CreateAssetMenu(fileName = "DialogConfig", menuName = "ScriptableObjects/DialogConfig")]
public class DialogConfig : ScriptableObject
{
    public SerializableDictionaryBase<EDialogSound, AudioClip> soundDict;

    public bool overrideSounds = false, overrideColors = false, overrNextSentTiming = false, 
        overrTimePerCharacter = false, overridePitch = false;
    
    public Color textColor = Color.white;
    public Color highlightColor = Color.red;

    public Color highlightColor2 = Color.blue;

    bool overrInvisChars = false;
    [Tooltip("Set to 'true' to avoid moving words mid-sentence.")]
    public bool invisibleCharacters = true;

    public KeyCode nextSentenceKey;
    public float nextSentenceSecs = 2f;

    public float timePerCharacter = 0.1f;

    public float pitch = 1f;
    public float pitchVariance = 0f;

    bool overrideWavyParams = false;
    [SerializeField] public float wavySpeed = 1f;
    public float wavyIntensity = 0.2f;
    public float wavyLetterOffset = 0.1f;

    // public DialogConfig(){}

    public DialogConfig Merge(DialogConfig _override){
        if(_override == null) { return this; }
        DialogConfig newConf = ScriptableObject.CreateInstance<DialogConfig>();
        newConf.soundDict = _override.overrideSounds ? _override.soundDict : this.soundDict;
        newConf.invisibleCharacters = _override.overrInvisChars ? _override.invisibleCharacters : this.invisibleCharacters;

        newConf.textColor = _override.overrideColors ? _override.textColor : this.textColor;
        newConf.highlightColor = _override.overrideColors ? _override.highlightColor : this.highlightColor;

        newConf.nextSentenceKey = _override.overrNextSentTiming ? _override.nextSentenceKey : this.nextSentenceKey;
        newConf.nextSentenceSecs = _override.overrNextSentTiming ? _override.nextSentenceSecs : this.nextSentenceSecs;

        newConf.timePerCharacter = _override.overrTimePerCharacter ? _override.timePerCharacter : this.timePerCharacter;

        newConf.pitch = _override.overridePitch ? _override.pitch : this.pitch;
        newConf.pitchVariance = _override.overridePitch ? _override.pitch : this.pitch;

        newConf.wavySpeed = _override.overrideWavyParams ? _override.wavySpeed : this.wavySpeed;
        newConf.wavyIntensity = _override.overrideWavyParams ? _override.wavyIntensity : this.wavyIntensity;
        newConf.wavyLetterOffset = _override.overrideWavyParams ? _override.wavyLetterOffset : this.wavyLetterOffset;
        return newConf;
    }
}