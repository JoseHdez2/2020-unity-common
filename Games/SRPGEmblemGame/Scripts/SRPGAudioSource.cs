using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESRPGSound {
    MOVE,
    TOGGLE,
    ACCEPT,
    CANCEL,
    BUZZER
}

[RequireComponent(typeof(AudioSource))]
public class SRPGAudioSource : AudioSourceMultiBase<ESRPGSound>
{}
