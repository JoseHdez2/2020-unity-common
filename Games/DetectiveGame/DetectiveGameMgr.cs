using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveGameMgr : MonoBehaviour
{
    [SerializeField] private AnimFade fadeOut; 
    [SerializeField] private DialogueManager dialogueManager;
    // Start is called before the first frame update
    [SerializeField] private Dialogue dialog1;
    void Start()
    {
        fadeOut.Toggle(true);
        StartCoroutine(CrStart());
    }

    public IEnumerator CrStart(){
        yield return new WaitForSeconds(0.5f);
        dialogueManager.WriteDialogue(dialog1);
        yield return new WaitUntil(() => dialogueManager.isDone);
    }
}
