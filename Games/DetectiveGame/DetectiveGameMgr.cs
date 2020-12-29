using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using ExtensionMethods;
using System;

public class DetectiveGameMgr : MonoBehaviour
{
    [SerializeField] private AnimFade fadeOut; 
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogBubbleUI nameBubble;
    // Start is called before the first frame update
    
    public TextAsset dialogJson01;
    private string[] dialog01;
    int dialogInd;
    [SerializeField] private Dialogue dialog1, dialog2;
    void Start(){
        fadeOut.Toggle(true);
        dialog01 = dialogJson01.text.Split('\n');
    }

    private void Update() {
        if(fadeOut.IsDone() && dialogueManager.isDone && dialogInd < dialog01.Length){
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
            if (name != null) {
                name = name.Trim();
                nameBubble.WriteSentence(name);
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
                default: break;
            }
        }
        line = pattern.Replace(line, "");        
        return line;
    }

    private void FadeIn()
    {
        fadeOut.Toggle(false);        
    }

    private void FadeOut()
    {
        fadeOut.Toggle(true);
    }

    public IEnumerator CrStart(){
        dialogueManager.WriteDialogue(dialog1);
        yield return new WaitUntil(() => dialogueManager.isDone);
        fadeOut.Toggle(false);
        yield return new WaitUntil(() => fadeOut.IsDone());
        dialogueManager.WriteDialogue(dialog2);
    }
}
