using UnityEngine;

public enum EDetectiveSound { Blink, Clue, Shake };

[RequireComponent(typeof(AudioSource))]
public class AudioSourceDetective : AudioSourceMultiBase<EDetectiveSound> { }