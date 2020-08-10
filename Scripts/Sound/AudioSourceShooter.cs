using UnityEngine;
using System.Collections;

// TODO remove this comment.
public enum EShooterSound
{
    SHOOT,
    CANT_SHOOT,
    RELOAD,
}

[RequireComponent(typeof(AudioSource))]
public class AudioSourceShooter : AudioSourceMultiBase<EShooterSound>{}
