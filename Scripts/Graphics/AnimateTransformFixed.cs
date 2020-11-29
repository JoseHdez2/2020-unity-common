using UnityEngine;
using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;
using System;
using System.Collections.Generic;

public class AnimateTransformFixed : MonoBehaviour {
    [SerializeField] public List<MyTransformAnimation> anims;

    // Update is called once per frame
    void FixedUpdate()
    {
        anims.ForEach(anim => UpdateAnim(anim));
    }

    private void UpdateAnim(MyTransformAnimation anim)
    {
        float value = anim.curve.Evaluate((Time.time * anim.speed) % 1);

        value *= anim.valueRange;
        switch (anim.attr) {
            case Attribute.POS_X: transform.position = new Vector3(value, 0, 0); break;
            case Attribute.POS_Y: transform.position = new Vector3(0, value, 0); break;
            case Attribute.SCALE: transform.localScale = new Vector3(0, value, 0); break;
            case Attribute.ROT_Z: transform.RotateAround(transform.position, transform.forward, value); break;
        }
    }
}
