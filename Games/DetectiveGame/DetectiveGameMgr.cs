using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using ExtensionMethods;
using System;

public class DetectiveGameMgr : MonoBehaviour
{
    [SerializeField] private AudioSourceDetective audioSourceDetective; 
    [SerializeField] private AnimFade blackScreen, whiteScreen;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogBubbleUI nameBubble;
    // Start is called before the first frame update
    
    public TextAsset dialogJson01;
    private string[] dialog01;
    int dialogInd;
    public ObjectShake objectShake;
    [SerializeField] private Dialogue dialog1, dialog2;
    void Start(){
        blackScreen.Toggle(true);
        dialog01 = dialogJson01.text.Split('\n');
    }

    private void Update() {
        if(blackScreen.IsDone() && dialogueManager.isDone && dialogInd < dialog01.Length){
            ProcessNewLine();
        }
    }

    private void ProcessNewLine()
    {
        string line = dialog01[dialogInd];
        string dialog;
        line = ProcessCommands(line);
        if (line.Contains(":")) {
            string name = line.Split(':')[0];
            dialog = line.Split(':')[1];
            if (!name.IsNullOrWhiteSpace()) {
                name = name.Trim();
                if(name == "Narrator"){
                    nameBubble.Toggle(false);
                } else {
                    nameBubble.WriteSentence(name);
                }
            }
        } else {
            dialog = line;
        }        
        dialog = dialog.Trim();
        if (!dialog.IsNullOrWhiteSpace()) {
            dialogueManager.WriteOneShot(dialog);
        }
        dialogInd++;
    }

    private string ProcessCommands(string line)
    {
        Regex pattern = new Regex(@"\[.+\]");
        Match match = pattern.Match(line);
        if (match != null)
        {
            string command = match.Value;
            Debug.Log(command);
            switch(command){
                case "[fade in]": FadeIn(); break;
                case "[fade out]": FadeOut(); break;
                case "[blink]": Blink(); break;
                case "[shake]": Shake(); break;
                default: break;
            }
        }
        line = pattern.Replace(line, "");        
        return line;
    }

    private void Blink()
    {
        whiteScreen.ToggleFor(true, 0.25f);
        audioSourceDetective.PlaySound(EDetectiveSound.Blink);
    }

    private void FadeIn()
    {
        blackScreen.Toggle(false);        
    }

    private void FadeOut() {
        blackScreen.Toggle(true);
    }

    private void Shake() {
        objectShake.Shake();
        audioSourceDetective.PlaySound(EDetectiveSound.Shake);
    }

    public IEnumerator CrStart(){
        dialogueManager.WriteDialogue(dialog1);
        yield return new WaitUntil(() => dialogueManager.isDone);
        blackScreen.Toggle(false);
        yield return new WaitUntil(() => blackScreen.IsDone());
        dialogueManager.WriteDialogue(dialog2);
    }
}
