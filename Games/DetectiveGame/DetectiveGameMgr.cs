using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using ExtensionMethods;
using System;
using System.Linq;
using UnityEngine.UI;

[Serializable]
public class JsonDialog {
    [SerializeField] public JsonDialogLine[] lines;
    public int index;
    public int Length() => lines.Length;
}

[Serializable]
public class JsonDialogLine {
    [SerializeField] public string raw;
    [SerializeField] public string actor;
    [SerializeField] public string dialog;
    [SerializeField] public JsonDialogOption[] options;
}

[Serializable]
public class JsonDialogOption : JsonDialog {
    [SerializeField] public string option;
}

public class DetectiveGameMgr : MonoBehaviour
{
    [SerializeField] private AudioSourceDetective audioSourceDetective; 
    [SerializeField] private AnimFade blackScreen, whiteScreen, charFade;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogBubbleUI nameBubble;
    [SerializeField] private Image charImage;
    
    public TextAsset[] dialogs;
    public Texture2D[] charImages;
    private Stack<JsonDialog> dialogStack = new Stack<JsonDialog>();
    public ObjectShake objectShake;
    void Start(){
        blackScreen.Toggle(true);
        dialogStack.Push(JsonUtility.FromJson<JsonDialog>(dialogs[0].text));
    }

    private JsonDialog curJsonDialog() => dialogStack.Peek();

    private void Update() {
        // if(blackScreen.IsDone() && dialogueManager.isDone && dialogInd < curDialog.Length){
        //     ProcessNewLine();
        // }
        if(blackScreen.IsDone() && dialogueManager.isDone && curJsonDialog().index < curJsonDialog().Length()){
            ProcessNewLine();
        }
    }

    // new
    private void ProcessNewLine() {
        JsonDialogLine line = curJsonDialog().lines[curJsonDialog().index];
        if(line.options != null && !line.options.IsEmpty()){
            Debug.Log("options!");
        } else { 
            if(!line.raw.IsNullOrWhiteSpace()) {
                line = ParseLine(line.raw);
            }
            ProcessLine(line.actor, line.dialog);
        }
        curJsonDialog().index++;
    }

    private JsonDialogLine ParseLine(string rawLine) {
        JsonDialogLine line = new JsonDialogLine();
        rawLine = ProcessCommands(rawLine);
        if (rawLine.Contains(":")) {
            line.actor = rawLine.Split(':')[0].Trim();
            line.dialog = rawLine.Split(':')[1].Trim();
        } else {
            line.dialog = rawLine;
        }
        return line;
    }

    private void ProcessLine(string actor, string dialog) {
        if (!actor.IsNullOrWhiteSpace()) {
            if(actor == "Narrator"){
                nameBubble.Toggle(false);
            } else {
                nameBubble.WriteSentence(actor);
            }
        }
        if (!dialog.IsNullOrWhiteSpace()) {
            dialogueManager.WriteOneShot(dialog);
        }
    }

    private string ProcessCommands(string line) {
        Regex pattern = new Regex(@"\[.+\]");
        Match match = pattern.Match(line);
        if (match.Success) {
            string command = match.Value;            
            if (command.Contains("goto")){
                string filename = new Regex(@"\[\w+\s+([\w-.]+)\]").Match(command).Groups[1].Captures[0].ToString();                
                Debug.Log(filename);
                dialogGoto(filename);
            } else if (command.Contains("show")) {
                string filename = new Regex(@"\[\w+\s+([\w-.]+)\]").Match(command).Groups[1].Captures[0].ToString();                
                Debug.Log(filename);
                StartCoroutine(CrShowChar(filename));
            } else {
                switch(command){
                    case "[fade in]": FadeIn(); break;
                    case "[fade out]": FadeOut(); break;
                    case "[hide char]": StartCoroutine(CrHideChar()); break;
                    case "[blink]": Blink(); break;
                    case "[shake]": Shake(); break;
                    default: Debug.LogError($"Command {command} doesn't exist!"); break;
                }
            }
        }
        line = pattern.Replace(line, "");        
        return line;
    }

    private IEnumerator CrShowChar(string filename){
        charFade.Toggle(false);
        yield return new WaitUntil(charFade.IsDone);

        Texture2D image = charImages.ToList().First(d => d.name == filename);
        charImage.sprite = Sprite.Create(image, charImage.sprite.rect, charImage.sprite.pivot);
        
        charFade.Toggle(true);
        yield return new WaitUntil(charFade.IsDone);
    }

    private IEnumerator CrHideChar(){
        charFade.Toggle(false);
        yield return new WaitUntil(charFade.IsDone);
    }

    private void dialogGoto(string filename) {
        TextAsset dialog = dialogs.ToList().First(d => d.name == filename);
        dialogStack.Pop();
        dialogStack.Push(JsonUtility.FromJson<JsonDialog>(dialog.text));
    }

    private void Blink() {
        whiteScreen.ToggleFor(true, 0.25f);
        audioSourceDetective.PlaySound(EDetectiveSound.Blink);
    }

    private void FadeIn() {
        blackScreen.Toggle(false);        
    }

    private void FadeOut() {
        blackScreen.Toggle(true);
    }

    private void Shake() {
        objectShake.Shake();
        audioSourceDetective.PlaySound(EDetectiveSound.Shake);
    }
}
