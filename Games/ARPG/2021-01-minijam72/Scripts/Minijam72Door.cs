using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Minijam72Door : SpritePopInOut
{
    public EMinijam72KeyType keyType;
    AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        ArpgPlayerBehavior player = collision.gameObject.GetComponent<ArpgPlayerBehavior>();
        Minijam72Key key = FindObjectsOfType<Minijam72Key>().ToList()
            .FirstOrDefault(k => k.target == player.transform && k.keyType == keyType);
        if(key){
            Destroy(key.gameObject);
            audioSource.Play();
            SelfDestroy();
        }
    }
}
