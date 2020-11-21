using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESrpgSound {
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
    FanfareWin,
    FanfareLose,
    Miss
}

[RequireComponent(typeof(AudioSource))]
public class SrpgAudioSource : AudioSourceMultiBase<ESrpgSound>
{}
