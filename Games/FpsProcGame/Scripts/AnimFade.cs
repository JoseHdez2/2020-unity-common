using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeOneTime { ALPHA }

public class AnimationOneTime {
    public bool isFinished = true;
    [SerializeField] public AttributeOneTime attr;
    [SerializeField] public AnimationCurve valueCurve = AnimationCurve.EaseInOut(0, 0, 0, 1);
    [SerializeField] public RangeFloat valueRange = new RangeFloat(0f, 1f);
    [SerializeField] public float speed = 1f;
}

public class AnimFade : MonoBehaviour
{
    [SerializeField] public List<AnimationOneTime> anims;
    // Start is called before the first frame update
    void FixedUpdate(){
        // anims.ForEach(anim => UpdateAnim(anim));
    }

    private void UpdateAnim(AnimationCycle anim){
        float value = anim.curve.Evaluate((Time.time * anim.speed) % 1);

        value *= anim.valueRange;
        switch (anim.attr) {
            case AttributeCycle.POS_X: transform.position = new Vector3(value, 0, 0); break;
            case AttributeCycle.POS_Y: transform.position = new Vector3(0, value, 0); break;
            case AttributeCycle.SCALE: transform.localScale = new Vector3(0, value, 0); break;
            case AttributeCycle.ROT_Y: transform.RotateAround(transform.position, transform.up, value); break;
            case AttributeCycle.ROT_Z: transform.RotateAround(transform.position, transform.forward, value); break;
        }
    }
}
