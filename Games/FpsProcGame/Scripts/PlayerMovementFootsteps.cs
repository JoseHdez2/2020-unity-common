﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class PlayerMovementFootsteps : MonoBehaviour
{
    [System.Serializable]
    class FootstepSound {
        public string type;
        public AudioClip clip;
    }
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<FootstepSound> footstepSounds;
    [SerializeField] PlayerMovement playerMovement;
    private bool checkMove = true;
    private bool leftFoot = true;

    // Update is called once per frame
    void Update(){
        if(checkMove && playerMovement.IsWalking()){
            StartCoroutine(PlayFootstep(playerMovement.MoveMagnitude()));
        }
    }
    
    private IEnumerator PlayFootstep(float velocity){
        checkMove = false;
        audioSource.panStereo = leftFoot ? -0.5f : 0.5f;
        audioSource.clip = footstepSounds.Select(fs => fs.clip).ToList().RandomItem();
        audioSource.Play();
        leftFoot = !leftFoot;
        yield return new WaitForSeconds(1 - velocity * 0.5f);
        checkMove = true;
    }
}
