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
    public Color textColor = Color.white;
    public Color highlightColor = Color.red;

    [Tooltip("Set to 'true' to avoid moving words mid-sentence.")]
    public bool invisibleCharacters = true;
    public KeyCode nextSentenceKey;
    public float nextSentenceSecs = 2f;

    public float timePerCharacter = 0.1f;
    public float pitch = 1f;
    public float pitchVariance = 0f;

    public float wavySpeed = 1f;
    public float wavyIntensity = 0.2f;
    public float wavyLetterOffset = 0.1f;
}