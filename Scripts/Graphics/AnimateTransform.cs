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
    [SerializeField] public float valueRange = 0f;
}

public class AnimateTransform : MonoBehaviour {
    [SerializeField] public List<MyTransformAnimation> anims;
    private Vector3 initialPos, initialScale;
    private Quaternion initialRot;

    // Use this for initialization
    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
        transform.localScale = initialScale;
        anims.ForEach(anim => UpdateAnim(anim));
    }

    private void UpdateAnim(MyTransformAnimation anim)
    {
        float value = anim.curve.Evaluate(Time.time % 1);
        switch (anim.attr) {
            case Attribute.POS_X: transform.position += new Vector3(value, 0, 0); break;
            case Attribute.POS_Y: transform.position += new Vector3(0, value, 0); break;
            case Attribute.SCALE: transform.localScale += new Vector3(0, value, 0); break;
            case Attribute.ROT_Z: transform.rotation = Quaternion.Euler(new Vector3(0, 0, value)); break;
        }
    }
}
