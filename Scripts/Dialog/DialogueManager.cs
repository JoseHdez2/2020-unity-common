using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using ExtensionMethods;

public class DialogueManager : MonoBehaviour
{
    public Image dialogPanel;
    public TMP_Text dialogText;
    public TMP_Text dialogNpcNameText;
    public Image dialogPromptImg;
    public string highlightColor = "red";

    public AudioSource charSound;
    [Tooltip("Set to 'true' to avoid moving words mid-sentence.")]
    public bool invisibleCharacters = true;
    public KeyCode nextSentenceKey;
    public float nextSentenceSecs = 2f;

    void Start(){
        ShowPanelAndText(false);
    }

    static Coroutine dialogueCoroutine;

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

    public void WriteSentence(Dialogue dialogue, int iSent)
    {
        if (dialogue.sentences.Count == 0 || iSent >= dialogue.sentences.Count) { Stop(); return; }
        if (charSound) { charSound.pitch = dialogue.pitch; }
        if (dialogNpcNameText) { dialogNpcNameText.text = dialogue.name; }
        if (dialogPromptImg) { dialogPromptImg.enabled = false; }
        
        dialogueCoroutine = StartCoroutine(UpdateText(dialogue, iSent, 0));
    }

    public string Highlight(string str) => str.Color(highlightColor);

    IEnumerator UpdateText(Dialogue dialogue, int iSent, int iChar)
    {
        if (DialogueHasFinished(dialogue, iSent, iChar))
        {
            if (nextSentenceKey != KeyCode.None) {
                StartCoroutine(WaitForPlayerOk(dialogue, iSent));
            } else {
                StartCoroutine(WaitForNextSeconds(dialogue, iSent));
            }
            yield break;
        }
        string text = dialogue.sentences[iSent].Substring(0, iChar);
        text = Regex.Replace(text, "\\*(.+?)\\*", Highlight("$1"));
        dialogText.text = Regex.Replace(text, "\\*(.+?)?$", Highlight("$1"));
        if (invisibleCharacters){
            dialogText.text += $"<color=#00000000>{dialogue.sentences[iSent].Substring(iChar)}</color>";
        }
        yield return new WaitForSeconds(dialogue.timePerCharacter);
        if (charSound) { PlayCharSound(dialogue); }
        StartCoroutine(UpdateText(dialogue, iSent, iChar += 1));
    }

    private static bool DialogueHasFinished(Dialogue dialogue, int iSent, int iChar) 
        => (iChar >= dialogue.sentences[iSent].Length+1);

    void PlayCharSound(Dialogue dialogue)
    {
        if (dialogue.pitchVariance != 0){
            charSound.pitch = dialogue.pitch + Random.Range(-dialogue.pitchVariance, dialogue.pitchVariance);
        }
        charSound.Play();
    }

    IEnumerator WaitForPlayerOk(Dialogue dialogue, int iSent)
    {
        if (dialogPromptImg) { dialogPromptImg.enabled = true; }
        yield return new WaitUntil(() => Input.GetKeyDown(nextSentenceKey)); // TODO GetButton
        WriteSentence(dialogue, ++iSent);
    }

    IEnumerator WaitForNextSeconds(Dialogue dialogue, int iSent)
    {
        if (dialogPromptImg) { dialogPromptImg.enabled = true; }
        yield return new WaitForSeconds(nextSentenceSecs);
        WriteSentence(dialogue, ++iSent);
    }
}
