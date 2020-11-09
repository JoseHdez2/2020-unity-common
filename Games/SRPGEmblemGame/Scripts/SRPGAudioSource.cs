using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESRPGSound {
    FieldCursor,
    Toggle,
    Accept,
    Cancel,
    Buzzer,
    SelectUnit,
    MenuCursor,
    UnitPrompt,
    UnitFootsteps
}

[RequireComponent(typeof(AudioSource))]
public class SRPGAudioSource : AudioSourceMultiBase<ESRPGSound>
{}
