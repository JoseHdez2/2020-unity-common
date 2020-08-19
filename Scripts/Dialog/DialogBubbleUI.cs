using UnityEngine;
using System.Collections;
using TMPro;
using System.Text.RegularExpressions;
using ExtensionMethods;
using System;
using UnityEngine.UI;

public class DialogBubbleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private ImageWipe imageWipe;

    private DialogConfig config = null;
    private string text = "";

    public AudioSourceDialog audioSource;
    public Image promptImage;
    public DialogueManager dialogManager;

    [Tooltip("Wipe down and back up for each new text we display.")]
    public bool wipeOnNewText = false;
    [Tooltip("Can talk back to the dialogManager to receive new text.")]
    public bool canAskForMoreText = false;

    private int charIndex;

    public IEnumerator WriteSentence(string text, DialogConfig config = null) {
        this.text = text;
        if (config) { this.config = config; }
        // ShowPanelAndText(true);
        charIndex = 0;
        tmpText.text = "";
        if (wipeOnNewText) {
            imageWipe.ToggleWipeFast(false);
            yield return new WaitUntil(() => imageWipe.isDone);
        }
        imageWipe.ToggleWipe(true);

        if (audioSource) { audioSource.SetPitch(config.pitch); }
        if (promptImage) { promptImage.enabled = false; }

        StartCoroutine(UpdateText(0));
    }

    public void ShowPanelAndText(bool show) {
        if (show) {
            audioSource.PlaySound(EDialogSound.WindowShow);
        } else {
            audioSource.PlaySound(EDialogSound.WindowHide);
            tmpText.text = "";
        }
        if (imageWipe) { imageWipe.ToggleWipe(show); }
    }

    public bool IsWipeDone() => imageWipe.isDone;
    
    private void Update() {
        if (config && imageWipe.wipeMode == ImageWipe.WipeMode.Filled){
            DrawText();
        }
    }

    private void DrawText(){
        string str = $"<line-height=100%><color={config.textColor.ToRGBA()}>";
        try {
            str += text.Substring(0, charIndex);
        } catch (Exception e) {
            Debug.Log($"iChar: {charIndex}");
            throw e;
        }
        str = Regex.Replace(str, "\\*(.+?)\\*", m => Highlight(m.Groups[1].ToString()));
        str = Regex.Replace(str, "~(.+?)~", m => Wavy(m.Groups[1].ToString()));
        str = Regex.Replace(str, "\\*(.+?)?$", Highlight("$1"));
        str = Regex.Replace(str, "~(.+?)?$", m => Wavy(m.Groups[1].ToString()));
        tmpText.text = str;
        if (config.invisibleCharacters) {
            string invisibleText = $"<color=#00000000>" +
                $"{text.Substring(charIndex)}</color>";
            invisibleText = invisibleText.Replace("*", "").Replace("~", "");
            tmpText.text += invisibleText;
        }
    }

    private IEnumerator UpdateText(int iChar) {
        yield return new WaitUntil(() => imageWipe.isDone);
        charIndex = iChar;
        if (IsDone()) {
            yield break;
        }
        yield return new WaitForSeconds(config.timePerCharacter);
        PlayCharSound();
        StartCoroutine(UpdateText(iChar += 1));
    }

    public void OnMouseDown()
    {
        Debug.Log("Mouse down!");
    }

    private void HandleFinished0()
    {
        audioSource.PlaySound(EDialogSound.FullStop);
        charIndex = text.Length;
        if (canAskForMoreText) {
            HandleFinished();
        }
    }

    private void HandleFinished() {
        if (config.nextSentenceKey != KeyCode.None) {
            StartCoroutine(WaitForPlayerOk());
        } else {
            StartCoroutine(WaitForNextSeconds());
        }
    }

    private void PlayCharSound() {
        float pitch = config.pitch;
        float pitchVariance = config.pitchVariance;
        if (pitchVariance != 0) {
            float pitchWithVariance = pitch + UnityEngine.Random.Range(-pitchVariance, pitchVariance);
            audioSource.PlaySoundWithPitch(EDialogSound.Char, pitchWithVariance);
        } else {
            audioSource.PlaySoundWithPitch(EDialogSound.Char, pitch);
        }
    }

    private IEnumerator WaitForPlayerOk() {
        if (promptImage) { promptImage.enabled = true; }
        yield return new WaitUntil(() => Input.GetKeyDown(config.nextSentenceKey)); // TODO GetButton
        audioSource.PlaySound(EDialogSound.Next);
        dialogManager.WriteNextSentence();
    }

    private IEnumerator WaitForNextSeconds() {
        if (promptImage) { promptImage.enabled = true; }
        yield return new WaitForSeconds(config.nextSentenceSecs);
        audioSource.PlaySound(EDialogSound.Next);
        dialogManager.WriteNextSentence();
    }

    private string Highlight(string str) => str.Color(config.highlightColor);

    private string Wavy(string str) =>
        str.Wavy(config.wavySpeed, config.wavyIntensity, config.wavyLetterOffset);

    private bool IsDone() => (charIndex >= text.Length);
}
