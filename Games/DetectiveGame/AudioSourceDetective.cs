using UnityEngine;

public enum EDetectiveSound { Blink, Clue, Pierce };

[RequireComponent(typeof(AudioSource))]
public class AudioSourceDetective : AudioSourceMultiBase<EDetectiveSound> {


 }