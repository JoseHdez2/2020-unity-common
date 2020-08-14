using ExtensionMethods;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogConfig defaultConfig;
    private DialogConfig nameConfig;

    public DialogBubbleUI dialogBubble;
    public DialogBubbleUI nameBubble;

    private Dialogue dialogue;

    private int bubbleIndex = 0;

    private string curName;

    private void Start() {
        nameConfig = new DialogConfig();
        nameConfig.pitch = 0;
        ShowPanelAndText(false);
    }

    private static Coroutine dialogueCoroutine;

    public void WriteDialogue(Dialogue dialogue) {
        this.dialogue = dialogue;
        bubbleIndex = 0;
        WriteSentence();
    }

    public void ShowPanelAndText(bool show) {
        dialogBubble.ShowPanelAndText(show);
        nameBubble.ShowPanelAndText(show);
    }

    public void Stop() {
        if (dialogueCoroutine != null) {
            StopAllCoroutines();
            dialogueCoroutine = null;
        }
        ShowPanelAndText(false);
    }

    public void WriteSentence() {
        if (DialogHasEnded(dialogue, bubbleIndex)) { Stop(); return; }
        DialogBubble dialogBubbleData = dialogue.dialogBubbles[bubbleIndex];
        DialogConfig config = (dialogBubbleData.config) ? 
            new DialogConfig(defaultConfig, dialogBubbleData.config) : defaultConfig;

        StartCoroutine(dialogBubble.WriteSentence(dialogBubbleData.text, config));

        if(curName != dialogBubbleData.name) {
            if (string.IsNullOrEmpty(dialogBubbleData.name)) {
                nameBubble.ShowPanelAndText(false);
            } else {
                StartCoroutine(nameBubble.WriteSentence(dialogBubbleData.name, nameConfig));
            }
        }
        curName = dialogBubbleData.name;
    }

    public void WriteNextSentence() {
        bubbleIndex += 1;
        WriteSentence();
    }

    private static bool DialogHasEnded(Dialogue dialogue, int iSent) {
        return (dialogue.dialogBubbles.Count == 0 || iSent >= dialogue.dialogBubbles.Count);
    }
}