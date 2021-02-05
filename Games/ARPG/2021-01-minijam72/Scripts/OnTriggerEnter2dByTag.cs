using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Collider2dEvent : UnityEvent<Collider2D>{}

[Serializable]
public class TransformEvent : UnityEvent<Transform>{}

public class OnTriggerEnter2dByTag : MonoBehaviour {
    public string collTag;
    [SerializeField] Collider2dEvent triggerAction;
    private void OnTriggerEnter2D(Collider2D collision){
        if(collTag == collision.gameObject.tag){
            triggerAction.Invoke(collision);
        }
    }
}
