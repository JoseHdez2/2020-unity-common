using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(dialogueManager.isDone && dialogInd < dialog01.Length){
            ProcessNewLine();
        }
    }

    private void ProcessNewLine()
    {
        string line = dialog01[dialogInd];
        if(!line.Contains(":")){
            return;
        }
        string name = line.Split(':')[0];        
        string dialog = line.Split(':')[1];
        if(name != null){
            nameBubble.WriteSentence(name);            
        }
        if(dialog != null){
            dialogueManager.WriteOneShot(dialog);
        }
        dialogInd++;
    }














    public IEnumerator CrStart(){
        dialogueManager.WriteDialogue(dialog1);
        yield return new WaitUntil(() => dialogueManager.isDone);
        fadeOut.Toggle(false);
        yield return new WaitUntil(() => fadeOut.IsDone());
        dialogueManager.WriteDialogue(dialog2);
    }
}
