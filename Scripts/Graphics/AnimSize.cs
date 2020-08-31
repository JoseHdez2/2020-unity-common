using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSize : MonoBehaviour
{
    [Range(0, 10)]
    public float sizeRange = 0.5f;
    public float offset = 0f;
    [Range(0, 10)]
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        float time = (offset + Time.time) * speed;
        float curScale = 1 + Mathf.Sin(time) * sizeRange;
        transform.localScale = new Vector3(curScale, curScale, curScale);
    }
}
