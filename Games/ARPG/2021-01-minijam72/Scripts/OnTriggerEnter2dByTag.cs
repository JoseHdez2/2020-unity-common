using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnter2dByTag : MonoBehaviour
{
    public string collTag;
    [SerializeField] UnityEvent<Collider2D> triggerAction;
    private void OnTriggerEnter2D(Collider2D collision){
        if(collTag == collision.gameObject.tag){
            triggerAction.Invoke(collision);
        }
    }
}
