using System;
using System.Collections.Generic;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class ArpgAnimator : MonoBehaviour
{
    public ArpgPlayerBehavior behavior;
    public SpriteRenderer spriteRenderer;
    public SpriteAnimsTopdown spritesDict;
    private int index = 0;
    private RepeatingTimer timer;
    void Start() {
        timer = new RepeatingTimer(spritesDict.frameDuration);
    }

    // Update is called once per frame
    void Update() {
        spriteRenderer.flipX = behavior.GetFacingDirection() == spritesDict.flipDir;
        if(behavior.IsMoving()) {
            if(timer.UpdateAndCheck(Time.deltaTime)){
                timer.Reset();
                index++;
                List<Sprite> sprites = spritesDict.spritesDict[behavior.GetFacingDirection()].sprites;            
                index = sprites.WrapIndex(index);
                spriteRenderer.sprite = sprites[index];
            }
        }
    }
}
