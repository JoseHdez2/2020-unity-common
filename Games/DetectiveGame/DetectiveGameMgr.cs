using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveGameMgr : MonoBehaviour
{
    [SerializeField] private AnimFade fadeOut; 
    [SerializeField] private DialogueManager dialogueManager;
    // Start is called before the first frame update
    [SerializeField] private Dialogue dialog1, dialog2;
    void Start()
    {
        fadeOut.Toggle(true);
        StartCoroutine(CrStart());
    }

    public IEnumerator CrStart(){
        dialogueManager.WriteDialogue(dialog1);
        yield return new WaitUntil(() => dialogueManager.isDone);
        fadeOut.Toggle(false);
        yield return new WaitUntil(() => fadeOut.IsDone());
        dialogueManager.WriteDialogue(dialog2);
    }
}
