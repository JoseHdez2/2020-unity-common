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
    UnitFootsteps,
    TurnChange,
    Attack,
    UnitDeath,
    FanfareWin
}

[RequireComponent(typeof(AudioSource))]
public class SrpgAudioSource : AudioSourceMultiBase<ESRPGSound>
{}
