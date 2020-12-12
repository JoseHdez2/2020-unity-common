using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class PlayerMovementFootsteps : MonoBehaviour {
    [System.Serializable]
    class FootstepSoundList {
        public string type;
        [SerializeField] public List<AudioClip> clips;
    }
    [SerializeField] AudioSource audioSource;
    [SerializeField] float footstepDelayMax = 1, footstepDelayDiff = 0.5f;
    [SerializeField] List<FootstepSoundList> footstepSounds;
    [SerializeField] PlayerMovement playerMovement;
    private bool checkMove = true;
    private bool leftFoot = true;


    // Update is called once per frame
    void Update(){
        if(checkMove && playerMovement.enabled && playerMovement.IsWalking()){
            StartCoroutine(PlayFootstep(playerMovement.MoveMagnitude()));
        }
    }
    
    private IEnumerator PlayFootstep(float velocity){
        checkMove = false;
        audioSource.panStereo = leftFoot ? -0.5f : 0.5f;
        audioSource.clip = footstepSounds[0].clips.ToList().RandomItem();
        audioSource.Play();
        leftFoot = !leftFoot;
        yield return new WaitForSeconds(footstepDelayMax - velocity * footstepDelayDiff);
        checkMove = true;
    }
}
