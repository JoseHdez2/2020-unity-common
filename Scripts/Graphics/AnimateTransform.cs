using UnityEngine;
using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;
using System;
using System.Collections.Generic;

[Serializable]
public enum Attribute { POS_X, POS_Y, SCALE, ROT_Z }

[Serializable]
public class MyTransformAnimation {
    [SerializeField] public Attribute attr;
    [SerializeField] public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 0, 1);
    [SerializeField] public float speed = 1f;
    [SerializeField] public float valueRange = 1f;
}

public class AnimateTransform : MonoBehaviour {
    [SerializeField] public List<MyTransformAnimation> anims;

    private Dictionary<Attribute, float> prevValues = new Dictionary<Attribute, float>();
    private Dictionary<Attribute, float> prevTimes = new Dictionary<Attribute, float>();

    // Use this for initialization
    void Start()
    {
        anims.ForEach(anim => prevValues[anim.attr] = 0f);
        anims.ForEach(anim => prevTimes[anim.attr] = 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anims.ForEach(anim => UpdateAnim(anim));
    }

    private void UpdateAnim(MyTransformAnimation anim)
    {
        float time = Time.time;
        float value = anim.curve.Evaluate((Time.time * anim.speed) % 1);

        float timeDelta = time - prevTimes[anim.attr];
        float valueDelta = (value - prevValues[anim.attr]);

        prevValues[anim.attr] = value;
        prevTimes[anim.attr] = time;

        float delta = valueDelta * timeDelta * anim.valueRange;
        switch (anim.attr) {
            case Attribute.POS_X: transform.position += new Vector3(delta, 0, 0); break;
            case Attribute.POS_Y: transform.position += new Vector3(0, delta, 0); break;
            case Attribute.SCALE: transform.localScale += new Vector3(0, delta, 0); break;
            case Attribute.ROT_Z: transform.RotateAround(transform.position, Vector3.forward, delta); break;
        }
    }
}
