using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkColorTextPro : MonoBehaviour
{
    public TMP_Text text;
    public AnimationCycle anim;
    public Gradient gradient;

    // Update is called once per frame
    void Update()
    {
        UpdateAnim(anim);
    }

    private void UpdateAnim(AnimationCycle anim){
        float value = anim.curve.Evaluate((Time.time * anim.speed) % 1) * anim.valueRange;
        text.color = gradient.Evaluate(value);
    }
}
