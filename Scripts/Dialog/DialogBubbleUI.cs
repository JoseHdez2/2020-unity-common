using UnityEngine;
using System.Collections;
using TMPro;
using System.Text.RegularExpressions;
using ExtensionMethods;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DialogBubbleUI : MonoBehaviour
{
    enum State { NONE, WRITING, WAITING }
    private State state = State.NONE;

    // enum NextSentConfig { ONLY_INPUT, ONLY_TIMER, BOTH }
    // public NextSentConfig nextSentConfig
    // TODO: do this in the DialogConfig.

    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private ImageWipe imageWipe;

    [SerializeField] private DialogConfig bubbleConfig = null;
    private string fullText = "";

    public AudioSourceDialog audioSource;
    public Image promptImage;
    public DialogueManager dialogManager;

    [Tooltip("Wipe down and back up for each new text we display.")]
    public bool wipeOnNewText = false;
    [Tooltip("Can talk back to the dialogManager to receive new text.")]
    public bool canAskForMoreText = false;

    private int charIndex;
    private float secsBeforeNextSentence = float.MaxValue;

    public void WriteSentence(string text, DialogConfig newConfig = null){
        StartCoroutine(CrWriteSentence(text, newConfig));
    }
    private IEnumerator CrWriteSentence(string text, DialogConfig newConfig = null) {
        fullText = text;
        state = State.WRITING;
        if (newConfig) { bubbleConfig = newConfig; }
        // ShowPanelAndText(true);
        charIndex = 0;
        tmpText.text = "";
        if (wipeOnNewText) {
            imageWipe.ToggleFast(false);
            yield return new WaitUntil(() => imageWipe.IsDone());
        }
        imageWipe.Toggle(true);

        if (audioSource) { 
            audioSource.SetPitch(bubbleConfig.pitch);
            // audioSource.soundDict = bubbleConfig.soundDict;
        }
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
        if (imageWipe) { imageWipe.Toggle(show); }
    }

    public bool IsWipeDone() => imageWipe.IsDone();

    private void Update() {
        if (bubbleConfig && imageWipe.wipeMode == ImageWipe.WipeMode.Filled) {
            DrawText();
        }
        switch (state) {
            case State.WRITING:
                if (UserInputThisFrame()) {
                    if (canAskForMoreText) {
                        ChangeStateToWaiting();
                    } else {
                        state = State.NONE;
                    }
                }
                break;
            case State.WAITING:
                secsBeforeNextSentence -= Time.deltaTime;
                if (UserInputThisFrame() || secsBeforeNextSentence < 0) {
                    AskForNextSentence();
                }
                break;
            case State.NONE:
                break;
        }
    }

    private bool UserInputThisFrame() => Mouse.current.leftButton.wasPressedThisFrame
            || Keyboard.current.enterKey.wasPressedThisFrame;

    private void DrawText(){
        string str = $"<line-height=100%><color={bubbleConfig.textColor.ToRGBA()}>";
        try {
            str += fullText.Substring(0, charIndex);
        } catch (Exception e) {
            Debug.Log($"iChar: {charIndex}");
            throw e;
        }
        str = Regex.Replace(str, "\\*(.+?)\\*", m => Highlight(m.Groups[1].ToString()));
        str = Regex.Replace(str, "~(.+?)~", m => Wavy(m.Groups[1].ToString()));
        str = Regex.Replace(str, "\\*(.+?)?$", Highlight("$1"));
        str = Regex.Replace(str, "~(.+?)?$", m => Wavy(m.Groups[1].ToString()));
        tmpText.text = str;
        if (bubbleConfig.invisibleCharacters) {
            string invisibleText = $"<color=#00000000>" +
                $"{fullText.Substring(charIndex)}</color>";
            invisibleText = invisibleText.Replace("*", "").Replace("~", "");
            tmpText.text += invisibleText;
        }
    }

    private IEnumerator UpdateText(int iChar) {
        yield return new WaitUntil(() => imageWipe.IsDone());
        charIndex = iChar;
        if (IsDone()) {
            ChangeStateToWaiting();
            yield break;
        }
        if(iChar > 0) {
            switch (fullText[iChar - 1]) {
                case '.':
                case '!':
                case '?':
                    yield return new WaitForSeconds(bubbleConfig.timePerCharacter * 30);
                    break;
                case ',':
                    yield return new WaitForSeconds(bubbleConfig.timePerCharacter * 10);
                    break;
                default:
                    yield return new WaitForSeconds(bubbleConfig.timePerCharacter);
                    break;
            }
        }
        if(fullText[iChar] == '.')
        yield return new WaitForSeconds(bubbleConfig.timePerCharacter);
        PlayCharSound();
        StartCoroutine(UpdateText(iChar += 1));
    }

    private void ChangeStateToWaiting() {
        StopAllCoroutines();
        state = State.WAITING;
        audioSource.PlaySound(EDialogSound.FullStop);
        charIndex = fullText.Length;
        secsBeforeNextSentence = float.MaxValue;
        if (bubbleConfig.nextSentenceKey == KeyCode.None) {
            secsBeforeNextSentence = bubbleConfig.nextSentenceSecs;
        }
    }

    private void PlayCharSound() {
        float pitch = bubbleConfig.pitch;
        float pitchVariance = bubbleConfig.pitchVariance;
        if (pitchVariance != 0) {
            float pitchWithVariance = pitch + UnityEngine.Random.Range(-pitchVariance, pitchVariance);
            audioSource.PlaySoundWithPitch(EDialogSound.Char, pitchWithVariance);
        } else {
            audioSource.PlaySoundWithPitch(EDialogSound.Char, pitch);
        }
    }

    private void AskForNextSentence(){
        state = State.NONE;
        audioSource.PlaySound(EDialogSound.Next);
        if (dialogManager) { dialogManager.WriteNextSentence(); }
    }

    private string Highlight(string str) => str.Color(bubbleConfig.highlightColor);

    private string Wavy(string str) =>
        str.Wavy(bubbleConfig.wavySpeed, bubbleConfig.wavyIntensity, bubbleConfig.wavyLetterOffset);

    private bool IsDone() => (charIndex >= fullText.Length);
}
