using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
[CreateAssetMenu(fileName = "DialogConfig", menuName = "ScriptableObjects/DialogConfig")]
public class DialogConfig : ScriptableObject
{
    public SerializableDictionaryBase<EDialogSound, AudioClip> soundDict;
    public string highlightColor = "red";

    [Tooltip("Set to 'true' to avoid moving words mid-sentence.")]
    public bool invisibleCharacters = true;
    public KeyCode nextSentenceKey;
    public float nextSentenceSecs = 2f;

    public float wavySpeed = 1f;
    public float wavyIntensity = 0.2f;
    public float wavyLetterOffset = 0.1f;
}