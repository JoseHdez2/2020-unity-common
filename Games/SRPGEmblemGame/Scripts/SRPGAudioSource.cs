using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESRPGSound {
    Move,
    Toggle,
    Accept,
    Cancel,
    Buzzer,
    SelectUnit
}

[RequireComponent(typeof(AudioSource))]
public class SRPGAudioSource : AudioSourceMultiBase<ESRPGSound>
{}
