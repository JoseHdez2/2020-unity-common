using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMinijam72KeyType { SILVER, GOLD, CORPSE }

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class Minijam72Key : MovementFollowTarget
{
    public EMinijam72KeyType keyType;
    AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        ArpgPlayerBehavior player = collision.gameObject.GetComponent<ArpgPlayerBehavior>();
        if(player){
            target = player.transform;
            if(audioSource){
                audioSource.Play();
            }
        }
    }

}
