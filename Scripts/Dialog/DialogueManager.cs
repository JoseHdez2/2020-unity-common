using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using ExtensionMethods;
using System;

public class DialogueManager : MonoBehaviour
{
    public DialogConfig defaultConfig;
    private DialogConfig dialogBubbleConfig;

    // public Image dialogPanel;
    public ImageWipe dialogPanelWipe;
    public TMP_Text dialogText;
    public TMP_Text dialogNpcNameText;
    public Image dialogPromptImg;

    public AudioSourceDialog audioSource;

    void Start(){
        dialogBubbleConfig = new DialogConfig();
        ShowPanelAndText(false);
    }

    static Coroutine dialogueCoroutine;

    public void Update()
    {
        if (dialogPanelWipe.wipeMode != ImageWipe.WipeMode.Empty){
            DrawText();
        }
    }

    //public void WriteDialogue(string str){
    //    WriteDialogue(new Dialogue(new string[] { str }));
    //}

    public void WriteDialogue(Dialogue dialogue) {
        if (dialogText == null) { Debug.Log("Cannot display dialog: No reference to a Text!"); return; }
        ShowPanelAndText(true);
        WriteSentence(dialogue, 0);
    }

    public void ShowPanelAndText(bool show){
        if(show)
        {
            audioSource.PlaySound(EDialogSound.WindowShow);
        } else {
            audioSource.PlaySound(EDialogSound.WindowHide);
            dialogText.text = "";
        }
        // if (dialogPanel) { dialogPanel.enabled = show; }
        if (dialogPanelWipe) { dialogPanelWipe.ToggleWipe(show); }
    }

    public void Stop(){
        if (dialogueCoroutine != null){
            StopAllCoroutines();
            dialogueCoroutine = null;
        }
        ShowPanelAndText(false);
    }

    private Dialogue dialogue;
    private int iSent, iChar;

    public void WriteSentence(Dialogue dialogue, int iSent)
    {
        this.dialogue = dialogue;
        this.iSent = iSent;

        if (DialogHasEnded(dialogue, iSent)) { Stop(); return; }

        UpdateDialogBubbleConfig(dialogue, iSent);

        if (drawTextCoroutine != null) { StopCoroutine(drawTextCoroutine); drawTextCoroutine = null; }
        if (audioSource) { audioSource.SetPitch(dialogBubbleConfig.pitch); }
        if (dialogNpcNameText) { dialogNpcNameText.text = dialogue.dialogBubbles[iSent].name; }
        if (dialogPromptImg) { dialogPromptImg.enabled = false; }

        dialogueCoroutine = StartCoroutine(UpdateText(dialogue, iSent, 0));
    }

    private static bool DialogHasEnded(Dialogue dialogue, int iSent)
        => (dialogue.dialogBubbles.Count == 0 || iSent >= dialogue.dialogBubbles.Count);

    private static bool DialogBubbleHasFinished(Dialogue dialogue, int iSent, int iChar)
        => (iChar >= dialogue.dialogBubbles[iSent].text.Length);

    private void UpdateDialogBubbleConfig(Dialogue dialog, int iSent)
    {
        DialogConfig bubbleConfig = dialog.dialogBubbles[iSent].config;
        dialogBubbleConfig.pitch = (bubbleConfig != null) ? bubbleConfig.pitch : defaultConfig.pitch;
        dialogBubbleConfig.pitchVariance = (bubbleConfig != null) ? bubbleConfig.pitchVariance : defaultConfig.pitchVariance;
        dialogBubbleConfig.timePerCharacter = (bubbleConfig != null) ? bubbleConfig.timePerCharacter : defaultConfig.timePerCharacter;
        dialogBubbleConfig.textColor = (bubbleConfig != null) ? bubbleConfig.textColor : defaultConfig.textColor;
        dialogBubbleConfig.highlightColor = (bubbleConfig != null) ? bubbleConfig.highlightColor : defaultConfig.highlightColor;
    }

    public string Highlight(string str) => str.Color(dialogBubbleConfig.highlightColor);

    public string Wavy(string str) => 
        str.Wavy(defaultConfig.wavySpeed, defaultConfig.wavyIntensity, defaultConfig.wavyLetterOffset);

    private Coroutine drawTextCoroutine;

    IEnumerator UpdateText(Dialogue dialogue, int iSent, int iChar)
    {
        this.iChar = iChar;
        if (DialogBubbleHasFinished(dialogue, iSent, iChar))
        {
            HandleDialogBubbleFinished(dialogue, iSent);
            yield break;
        }
        DrawText();
        yield return new WaitForSeconds(dialogBubbleConfig.timePerCharacter);
        PlayCharSound(dialogue, iSent);
        StartCoroutine(UpdateText(dialogue, iSent, iChar += 1));
    }

    private void HandleDialogBubbleFinished(Dialogue dialogue, int iSent)
    {
        audioSource.PlaySound(EDialogSound.FullStop);
        if (defaultConfig.nextSentenceKey != KeyCode.None)
        {
            StartCoroutine(WaitForPlayerOk(dialogue, iSent));
        }
        else
        {
            StartCoroutine(WaitForNextSeconds(dialogue, iSent));
        }
    }

    private void DrawText()
    {
        string text = $"<line-height=100%><color={dialogBubbleConfig.textColor.ToRGBA()}>";
        text += dialogue.dialogBubbles[iSent].text.Substring(0, iChar);
        text = Regex.Replace(text, "\\*(.+?)\\*", m => Highlight(m.Groups[1].ToString()));
        text = Regex.Replace(text, "~(.+?)~", m => Wavy(m.Groups[1].ToString()));
        text = Regex.Replace(text, "\\*(.+?)?$", Highlight("$1"));
        text = Regex.Replace(text, "~(.+?)?$", m => Wavy(m.Groups[1].ToString()));
        dialogText.text = text;
        if (defaultConfig.invisibleCharacters)
        {
            string invisibleText = $"<color=#00000000>{dialogue.dialogBubbles[iSent].text.Substring(iChar)}</color>";
            invisibleText = invisibleText.Replace("*", "").Replace("~", "");
            dialogText.text += invisibleText;
        }
    }

    void PlayCharSound(Dialogue dialogue, int iSent)
    {
        float pitch = dialogBubbleConfig.pitch;
        float pitchVariance = dialogBubbleConfig.pitchVariance;
        if (pitchVariance != 0){
            float pitchWithVariance = pitch + UnityEngine.Random.Range(-pitchVariance, pitchVariance);
            audioSource.PlaySoundWithPitch(EDialogSound.Char, pitchWithVariance);
        } else {
            audioSource.PlaySoundWithPitch(EDialogSound.Char, pitch);
        }
    }

    IEnumerator WaitForPlayerOk(Dialogue dialogue, int iSent)
    {
        if (dialogPromptImg) { dialogPromptImg.enabled = true; }
        yield return new WaitUntil(() => Input.GetKeyDown(defaultConfig.nextSentenceKey)); // TODO GetButton
        audioSource.PlaySound(EDialogSound.Next);
        WriteSentence(dialogue, ++iSent);
    }

    IEnumerator WaitForNextSeconds(Dialogue dialogue, int iSent)
    {
        if (dialogPromptImg) { dialogPromptImg.enabled = true; }
        yield return new WaitForSeconds(defaultConfig.nextSentenceSecs);
        audioSource.PlaySound(EDialogSound.Next);
        WriteSentence(dialogue, ++iSent);
    }
}
