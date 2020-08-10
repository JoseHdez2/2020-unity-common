using UnityEngine;
using System.Collections;

public class DialogAdvancedRunner : MonoBehaviour
{
    public DialogAdvanced dialog;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerEnter2D");
            // StopDialog();
            iBeat = 0;
            StartCoroutine(RunBeat());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerExit2D");
            StopDialog();
        }
    }

    private int iBeat = 0;

    internal IEnumerator RunBeat()
    {
        if(iBeat >= dialog.beats.Count) { yield break; }
        Debug.Log(iBeat);
        DialogBeatBad beat = dialog.beats[iBeat];
        StartCoroutine(beat.Run());
        yield return new WaitForSeconds(beat.secsBeforeNextBeat);
        iBeat = ++iBeat;
        StartCoroutine(RunBeat());
    }

    internal void StopDialog()
    {
        StopAllCoroutines();
    }
}
