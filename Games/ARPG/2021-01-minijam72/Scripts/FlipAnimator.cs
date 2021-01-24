using System;
using System.Collections.Generic;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class FlipAnimator : MonoBehaviour
{
    // public ArpgPlayerBehavior behavior;
    public SpriteRenderer spriteRenderer;

    public float frameDuration = 0.5f;
    private RepeatingTimer timer;
    void Start() {
        timer = new RepeatingTimer(frameDuration);
    }

    // Update is called once per frame
    void Update() {
        if(timer.UpdateAndCheck(Time.deltaTime)){
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
