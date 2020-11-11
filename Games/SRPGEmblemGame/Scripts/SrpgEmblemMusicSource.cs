using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESrpgEmblemMusic {
    PlayerTurn,
    EnemyTurn,
    FinalPush
}

[RequireComponent(typeof(AudioSource))]
public class SrpgEmblemMusicSource : AudioSourceMultiBase<ESrpgEmblemMusic>
{}
