using ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public DialogConfig defaultConfig;
    public DialogConfig nameConfig;

    public DialogBubbleUI dialogBubble;
    public DialogBubbleUI nameBubble;
    public List<Image> images;
    public List<Transform> imagePositions;

    [SerializeField] private GameObject[] disableDuringDialog;

    private Dialogue dialogue;
    private int bubbleIndex = 0;
    private string curName;

    private void Start() {
        ShowPanelAndText(false);
        isDone = true;
    }

    private static Coroutine dialogueCoroutine;
    public bool isDone;

    /// <summary>Write a string without the need to pass a Dialogue ScriptableObject.</summary>
    public void WriteOneShot(string text){
        Dialogue dialog = new Dialogue();
        dialog.dialogBubbles = new List<DialogBubble>() { new DialogBubble() {text = text} };
        WriteDialogue(dialog);
    }

    public void WriteDialogue(Dialogue dialogue) {
        isDone = false;
        this.dialogue = dialogue;
        bubbleIndex = 0;
        curName = null;
        disableDuringDialog.ToList().ForEach(obj => obj.SetActive(false));
        WriteSentence();
    }

    public void ShowPanelAndText(bool show) {
        dialogBubble.ShowPanelAndText(show);
        if (nameBubble) { nameBubble.ShowPanelAndText(show); }
    }

    public IEnumerator Stop() {
        ShowPanelAndText(false);
        yield return new WaitUntil(() => dialogBubble.IsWipeDone());
        disableDuringDialog.ToList().ForEach(obj => obj.SetActive(true));
        if (dialogueCoroutine != null) {
            dialogueCoroutine = null;
            StopAllCoroutines();
        }
        isDone = true;
    }

    public void WriteSentence() {
        if (DialogHasEnded(dialogue, bubbleIndex)) { StartCoroutine(Stop()); return; }
        DialogBubble dialogBubbleData = dialogue.dialogBubbles[bubbleIndex];
        DialogConfig sentConfig = defaultConfig.Merge(dialogBubbleData.config);

        if(dialogBubbleData.spriteIndex > -1) {
            images[(int)dialogBubbleData.pos].sprite = dialogue.sprites[dialogBubbleData.spriteIndex];
        }

        StartCoroutine(dialogBubble.WriteSentence(dialogBubbleData.text, sentConfig));

        if(nameBubble != null && curName != dialogBubbleData.name) {
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