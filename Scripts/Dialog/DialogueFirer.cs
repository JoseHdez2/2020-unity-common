using UnityEngine;
using System.Collections;

public class DialogueFirer : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Dialogue dialogue;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            dialogueManager.WriteDialogue(dialogue);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueManager.Stop();
        }
    }
}
