using UnityEngine;
using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;
using System;
using System.Collections.Generic;

[Serializable]
public enum AttributeCycle { POS_X, POS_Y, SCALE, ROT_Y, ROT_Z }

[Serializable]
public class AnimationCycle {
    [SerializeField] public AttributeCycle attr;
    [SerializeField] public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 0, 1);
    [SerializeField] public float speed = 1f;
    [SerializeField] public float valueRange = 1f;
}

public class AnimateTransform : MonoBehaviour {
    [SerializeField] public List<AnimationCycle> anims;

    private Dictionary<AttributeCycle, float> prevValues = new Dictionary<AttributeCycle, float>();
    private Dictionary<AttributeCycle, float> prevTimes = new Dictionary<AttributeCycle, float>();

    // Use this for initialization
    void Start(){
        anims.ForEach(anim => prevValues[anim.attr] = 0f);
        anims.ForEach(anim => prevTimes[anim.attr] = Time.time);
    }

    // Update is called once per frame
    void FixedUpdate(){
        anims.ForEach(anim => UpdateAnim(anim));
    }

    private void UpdateAnim(AnimationCycle anim){
        float time = Time.time;
        float value = anim.curve.Evaluate((Time.time * anim.speed) % 1);

        float timeDelta = time - prevTimes[anim.attr];
        float valueDelta = (value - prevValues[anim.attr]);

        prevValues[anim.attr] = value;
        prevTimes[anim.attr] = time;

        float delta = valueDelta * timeDelta * anim.valueRange;
        switch (anim.attr) {
            case AttributeCycle.POS_X: transform.position += new Vector3(delta, 0, 0); break;
            case AttributeCycle.POS_Y: transform.position += new Vector3(0, delta, 0); break;
            case AttributeCycle.SCALE: transform.localScale += new Vector3(0, delta, 0); break;
            case AttributeCycle.ROT_Y: transform.RotateAround(transform.position, transform.up, delta); break;
            case AttributeCycle.ROT_Z: transform.RotateAround(transform.position, transform.forward, delta); break;
        }
    }
}
