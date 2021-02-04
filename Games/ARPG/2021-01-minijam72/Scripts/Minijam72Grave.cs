using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Minijam72Grave : SpritePopInOut {
    public EMinijam72KeyType keyType;
    [SerializeField] GameObject spawnOnBury;
    AudioSource audioSource;
    private ArpgAction buryAction;
    private Minijam72Key key = null;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        buryAction = FindObjectsOfType<ArpgAction>().ToList().First(a => a.actionName == "bury");
    }

    private void OnTriggerEnter2D(Collider2D collision){
        ArpgPlayerBehavior player = collision.gameObject.GetComponent<ArpgPlayerBehavior>();
        if(player){
            key = FindObjectsOfType<Minijam72Key>().ToList()
                .FirstOrDefault(k => k.target != null && k.keyType == keyType);
            buryAction.canDo = key != null;
            buryAction.SetAction(Bury);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        ArpgPlayerBehavior player = collision.gameObject.GetComponent<ArpgPlayerBehavior>();
        if(player){
            buryAction.canDo = false;
        }
    }

    public void Bury(){
        if(key){
            Destroy(key.gameObject);
            audioSource.Play();
            Instantiate(spawnOnBury, transform.position, Quaternion.identity);
            // SelfDestroy();
        }
    }
}
