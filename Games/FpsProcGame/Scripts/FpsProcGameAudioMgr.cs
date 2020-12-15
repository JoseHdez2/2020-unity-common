using UnityEngine;

public class FpsProcGameAudioMgr : MonoBehaviour {
    [SerializeField] AudioSource ambianceSound, clockSound;

    public void StartClockSound(){
        clockSound.Play();
    }
}