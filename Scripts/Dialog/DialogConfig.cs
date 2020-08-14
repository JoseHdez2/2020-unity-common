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

    public bool overrideColors = false;
    public Color textColor = Color.white;
    public Color highlightColor = Color.red;

    bool overrInvisChars = false;
    [Tooltip("Set to 'true' to avoid moving words mid-sentence.")]
    public bool invisibleCharacters = true;

    bool overrNextSentTiming = false;
    public KeyCode nextSentenceKey;
    public float nextSentenceSecs = 2f;

    bool overrideTimePerCharacter = false;
    public float timePerCharacter = 0.1f;

    bool overridePitch = false;
    public float pitch = 1f;
    public float pitchVariance = 0f;

    bool overrideWavyParams = false;
    [SerializeField] public float wavySpeed = 1f;
    public float wavyIntensity = 0.2f;
    public float wavyLetterOffset = 0.1f;

    public DialogConfig(){}

    public DialogConfig(DialogConfig _default, DialogConfig _override){
        invisibleCharacters = _override.overrInvisChars ? _override.invisibleCharacters : _default.invisibleCharacters;

        nextSentenceKey = _override.overrNextSentTiming ? _override.nextSentenceKey : _default.nextSentenceKey;
        nextSentenceSecs = _override.overrNextSentTiming ? _override.nextSentenceSecs : _default.nextSentenceSecs;

        timePerCharacter = _override.overrideTimePerCharacter ? _override.timePerCharacter : _default.timePerCharacter;

        pitch = _override.overridePitch ? _override.pitch : _default.pitch;
        pitchVariance = _override.overridePitch ? _override.pitch : _default.pitch;

        wavySpeed = _override.overrideWavyParams ? _override.wavySpeed : _default.wavySpeed;
        wavyIntensity = _override.overrideWavyParams ? _override.wavyIntensity : _default.wavyIntensity;
        wavyLetterOffset = _override.overrideWavyParams ? _override.wavyLetterOffset : _default.wavyLetterOffset;
    }
}