using UnityEngine;
using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;
using System;
using System.Collections.Generic;

public class AnimateTransformFixed : MonoBehaviour {
    [SerializeField] public List<AnimationCycle> anims;

    // Update is called once per frame
    void FixedUpdate(){
        anims.ForEach(anim => UpdateAnim(anim));
    }

    private void UpdateAnim(AnimationCycle anim){
        float value = anim.curve.Evaluate((Time.time * anim.speed) % 1);

        value *= anim.valueRange;
        switch (anim.attr) {
            case AttributeCycle.POS_X: transform.position = new Vector3(value, 0, 0); break;
            case AttributeCycle.POS_Y: transform.position = new Vector3(0, value, 0); break;
            case AttributeCycle.SCALE: transform.localScale = new Vector3(0, value, 0); break;
            case AttributeCycle.SCALE_X: transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z); break;
            case AttributeCycle.SCALE_Y: transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z); break;
            case AttributeCycle.SCALE_Z: transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value); break;
            case AttributeCycle.ROT_Y: transform.RotateAround(transform.position, transform.up, value); break;
            case AttributeCycle.ROT_Z: transform.RotateAround(transform.position, transform.forward, value); break;
        }
    }
}
