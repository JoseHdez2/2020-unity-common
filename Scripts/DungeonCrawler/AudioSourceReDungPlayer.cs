using UnityEngine;
using System.Collections;

public enum EReDungPlayerSound
{
    STEP,
    BONK,
    LOCKED,
    UNLOCK,
    HURT,
    DIE,
    TIMEWARP1,
    TIMEWARP2
}

[RequireComponent(typeof(AudioSource))]
public class AudioSourceReDungPlayer : AudioSourceMultiBase<EReDungPlayerSound> {}
