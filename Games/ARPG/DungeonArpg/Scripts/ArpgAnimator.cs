using System;
using System.Collections.Generic;
using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[Serializable]
public class SpriteAnimation {
    [SerializeField] public List<Sprite> sprites;
}

public class ArpgAnimator : MonoBehaviour
{
    public ArpgPlayerBehavior behavior;
    public SpriteRenderer spriteRenderer;
    public SerializableDictionaryBase<EDirection, SpriteAnimation> spritesDict;
    public EDirection flipDir = EDirection.LEFT;
    public float frameDuration = 0.5f;
    private int index = 0;
    private RepeatingTimer timer;
    void Start() {
        timer = new RepeatingTimer(frameDuration);
    }

    // Update is called once per frame
    void Update() {
        spriteRenderer.flipX = behavior.GetFacingDirection() == flipDir;
        if(behavior.IsMoving()) {
            if(timer.UpdateAndCheck(Time.deltaTime)){
                timer.Reset();
                index++;
                List<Sprite> sprites = spritesDict[behavior.GetFacingDirection()].sprites;            
                index = sprites.WrapIndex(index);
                spriteRenderer.sprite = sprites[index];
            }
        }
    }
}
