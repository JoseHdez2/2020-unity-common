using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LevelMusic : Pausable
{
    private AudioSource audioSource;
    public AudioClip levelMusic;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = levelMusic;
        audioSource.Play();
    }

    protected override void Update2() { }
    protected override void FixedUpdate2() { }

    protected override void OnPause(bool isPaused)
    {
        if (isPaused){
            audioSource.Pause();
        } else{
            audioSource.UnPause();
        }
    }
}
