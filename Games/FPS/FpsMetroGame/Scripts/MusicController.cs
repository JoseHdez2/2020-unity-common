using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    public AudioSource aS;

    public List<AudioClip> songList;

    void Awake()
    {
        aS = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        aS.clip = songList[Random.Range(0, songList.Count)];
        aS.PlayOneShot(aS.clip);       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void toggle()
    {
        if (aS.isPlaying)
        {
            aS.Stop();
        }else {
            aS.clip = songList[Random.Range(0, songList.Count)];
            aS.PlayOneShot(aS.clip);
        }
    }

    //method that keeps selecting tracks [incomplete]
    /*
    private void switchTrack()
    {
        if (aS.clip.)
        {
            aS.clip = songList[Random.Range(0, songList.Count - 1)];
            aS.PlayOneShot(aS.clip);           
        }
    }
    */
}
