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
    public DialogConfig config;

    public Image dialogPanel;
    public TMP_Text dialogText;
    public TMP_Text dialogNpcNameText;
    public Image dialogPromptImg;

    public AudioSourceDialog audioSource;

    void Start(){
        ShowPanelAndText(false);
    }

    static Coroutine dialogueCoroutine;

    public void Update()
    {
        DrawText();
    }

    public void WriteDialogue(string str){
        WriteDialogue(new Dialogue(new string[] { str }));
    }

    public void WriteDialogue(Dialogue dialogue) {
        if (dialogText == null) { Debug.Log("Cannot display dialog: No reference to a Text!"); return; }
        ShowPanelAndText(true);
        WriteSentence(dialogue, 0);
    }

    public void ShowPanelAndText(bool show){
        if (!show) { dialogText.text = ""; }
        if (dialogPanel) { dialogPanel.enabled = show; } else
        {
            Debug.Log("Pizza");
        }
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

        if (drawTextCoroutine != null) { StopCoroutine(drawTextCoroutine); drawTextCoroutine = null; }
        if (dialogue.sentences.Count == 0 || iSent >= dialogue.sentences.Count) { Stop(); return; }
        if (audioSource) { audioSource.SetPitch(dialogue.pitch); }
        if (dialogNpcNameText) { dialogNpcNameText.text = dialogue.name; }
        if (dialogPromptImg) { dialogPromptImg.enabled = false; }
        
        dialogueCoroutine = StartCoroutine(UpdateText(dialogue, iSent, 0));
    }

    public string Highlight(string str) => str.Color(config.highlightColor);

    public string Wavy(string str) => 
        str.Wavy(config.wavySpeed, config.wavyIntensity, config.wavyLetterOffset);

    private Coroutine drawTextCoroutine;

    IEnumerator UpdateText(Dialogue dialogue, int iSent, int iChar)
    {
        this.iChar = iChar;
        if (DialogueHasFinished(dialogue, iSent, iChar))
        {
            if (config.nextSentenceKey != KeyCode.None)
            {
                StartCoroutine(WaitForPlayerOk(dialogue, iSent));
            }
            else
            {
                StartCoroutine(WaitForNextSeconds(dialogue, iSent));
            }
            yield break;
        }
        DrawText();
        yield return new WaitForSeconds(dialogue.timePerCharacter);
        PlayCharSound(dialogue);
        StartCoroutine(UpdateText(dialogue, iSent, iChar += 1));
    }

    private void DrawText()
    {
        string text = "<line-height=100%>" + dialogue.sentences[iSent].Substring(0, iChar);
        text = Regex.Replace(text, "\\*(.+?)\\*", m => Highlight(m.Groups[1].ToString()));
        text = Regex.Replace(text, "~(.+?)~", m => Wavy(m.Groups[1].ToString()));
        text = Regex.Replace(text, "\\*(.+?)?$", Highlight("$1"));
        text = Regex.Replace(text, "~(.+?)?$", m => Wavy(m.Groups[1].ToString()));
        dialogText.text = text;
        if (config.invisibleCharacters)
        {
            string invisibleText = $"<color=#00000000>{dialogue.sentences[iSent].Substring(iChar)}</color>";
            invisibleText = invisibleText.Replace("*", "").Replace("~", "");
            dialogText.text += invisibleText;
        }
    }

    private static bool DialogueHasFinished(Dialogue dialogue, int iSent, int iChar) 
        => (iChar >= dialogue.sentences[iSent].Length);

    void PlayCharSound(Dialogue dialogue)
    {
        if (dialogue.pitchVariance != 0){
            float pitch = dialogue.pitch + UnityEngine.Random.Range(-dialogue.pitchVariance, dialogue.pitchVariance);
            audioSource.PlaySoundWithPitch(EDialogSound.Char, pitch);
        } else {
            audioSource.PlaySound(EDialogSound.Char);
        }
    }

    IEnumerator WaitForPlayerOk(Dialogue dialogue, int iSent)
    {
        if (dialogPromptImg) { dialogPromptImg.enabled = true; }
        yield return new WaitUntil(() => Input.GetKeyDown(config.nextSentenceKey)); // TODO GetButton
        WriteSentence(dialogue, ++iSent);
    }

    IEnumerator WaitForNextSeconds(Dialogue dialogue, int iSent)
    {
        if (dialogPromptImg) { dialogPromptImg.enabled = true; }
        yield return new WaitForSeconds(config.nextSentenceSecs);
        WriteSentence(dialogue, ++iSent);
    }
}
