using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRainbowLerp : MonoBehaviour
{
    [Tooltip("Transition duration, in seconds.")]
    public float smoothing = 1f;
    [Tooltip("Steps that Color Lerp will take.")]
    public int colorSteps = 10;
    [Tooltip("How often, in seconds.")]
    public float frequency = 1f;

    private SpriteRenderer sprRend;
    void Awake()
    {
        sprRend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(ChangeToRandomColor());
    }

    IEnumerator ChangeToRandomColor()
    {
        Color target = Random.ColorHSV();
        float step = 1 / colorSteps;
        float stepSecs = frequency * step;
        while (sprRend.color != target)
        {
            sprRend.color = Color.Lerp(sprRend.color, target, smoothing);
            yield return new WaitForSeconds(stepSecs);
        }
        StartCoroutine(ChangeToRandomColor());
    }
}
