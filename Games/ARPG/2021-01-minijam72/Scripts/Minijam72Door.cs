using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Minijam72Door : MonoBehaviour
{
    AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
}
