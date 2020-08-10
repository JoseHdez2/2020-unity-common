using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDialogBeatType
{
    DIALOG, // display some text on-screen, then wait for player input.
    DIALOG_NO_PROMPT, // display some text and then automatically moves on to next beat.
    HEAL_PLAYER,
    DAMAGE_PLAYER,
    ADD_ITEM,
    REMOVE_ITEM,
    STOP
}

[System.Serializable]
public class DialogAdvanced
{
    // TODO public DialogueManager defaultDialogManager;
    public List<DialogBeatBad> beats;
    [SerializeField] public List<DialogBeat> beats2;
}

[System.Serializable]
public class DialogBeatBad
{
    public EDialogBeatType type;
    public float secsBeforeNextBeat = 0f;

    public Dialogue dialog;
    public DialogueManager dialogManager;

    // public PlayerDamageable player;
    public int healQty;
    public AudioClip sound;

    public IEnumerator Run()
    {
        Debug.Log(type);
        // TODO check if dialogManager == null and then get global "default" dialogManager?
        switch (type)
        {
            case EDialogBeatType.DIALOG: dialogManager.WriteDialogue(dialog); break;
            // case EDialogBeatType.HEAL_PLAYER: player.Heal(healQty); break;
        }
        yield break;
    }
}

[System.Serializable]
public abstract class DialogBeat : ScriptableObject
{
    public EDialogBeatType type;
    public float secsBeforeNextBeat = 0f;

    public abstract void RunBeat();
}

[System.Serializable]
public class DialogBeatDialog : DialogBeat
{
    new EDialogBeatType type = EDialogBeatType.DIALOG;

    public Dialogue dialog;
    public DialogueManager dialogManager;

    public override void RunBeat()
    {
        // TODO check if dialogManager == null and then get global "default" dialogManager?
        dialogManager.WriteDialogue(dialog);
    }
}

[System.Serializable]
public class DialogBeatHeal : DialogBeat
{
    new EDialogBeatType type = EDialogBeatType.HEAL_PLAYER;

    // PlayerDamageable player;
    public int healQty;
    public AudioClip sound;

    public override void RunBeat()
    {
        // player.Heal(healQty);
    }
}
